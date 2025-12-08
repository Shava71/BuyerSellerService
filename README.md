# ShopService — сервис покупок с метриками Prometheus и мониторингом Grafana

Этот проект реализует упрощённый backend-магазин с возможностью создания товаров, совершения покупок, публикацией доменных событий и системой сбора метрик.  
Сервис использует IOptionMonitor для хранерия статистики store-information, использует **MediatR** для событий, **Prometheus** для метрик и **Grafana** для визуализации.

---

## Технологии
- **.NET**
- **ASP.NET Core WebAPI**
- **Entity Framework Core**
- **Dapper**
- **MediatR** — доменные события
- **IOptionsMonitor** 
- **Prometheus-net** — метрики
- **Grafana** — визуализация
- **Fluent** - валидация
- **Swagger/OpenAPI**
- **xUnit + Moq** — тестирование

---

## Основные возможности

- CRUD товаров (создание, просмотр (одного или всех), обновление и удаление)
- Покупка одного или нескольких товаров
- Блокировка товара при покупке (Thread-safe за счёт FOR UPDATE PostgreSQL)
- Публикация доменного события (для уменьшения связности компонентов и более лёгкого масштабирования)
- Автоматическое измерение времени покупки
- Сбор метрик:
  - `shop_items_created_total`
  - `shop_purchases_total`
  - `shop_purchase_duration_seconds`
- Экспорт `/metrics` для Prometheus

---

## API

### SellerController — CRUD для Items

| Метод | Маршрут | Описание | Тело запроса | Ответ |
|------|---------|----------|--------------|--------|
| **GET** | `/api/seller/items` | Получить список всех товаров | — | `200 OK` + `Item` |
| **GET** | `/api/seller/items/{id}` | Получить товар по `id` | — | `200 OK` + `Item` или `404 NotFound` |
| **POST** | `/api/seller/items/create` | Создать новый товар | `CreateItemDto` | `200 OK` + `Item` |
| **PUT** | `/api/seller/items/update` | Обновить товар | `UpdateItemDto` | `200 OK` или `400 BadRequest` |
| **DELETE** | `/api/seller/items/delete/{id}` | Удалить товар | — | `200 OK` |

### BuyerController — Покупки и статистика

| Метод | Маршрут | Описание | Тело запроса | Ответ |
|------|---------|----------|--------------|--------|
| **POST** | `/api/buyer/purchase` | Совершить покупку товаров | `PurchaseRequest` | `200 OK` или `400 BadRequest` |
| **GET** | `/api/buyer/stats` | Получить статистику покупок | — | `200 OK` + `StatsConfig` |

### Пример хранящейся магазинной информации в IOptionsMonitor:
```
curl -X 'GET' \
  'http://localhost:8086/api/buyer/stats' \
  -H 'accept: */*'

Response body
Download
{
  "topCategory": "Jewelry",
  "averagePrice": 750.5
}
```

---

## Архитектура
```
ShopService
├── Application (Services, Dtos, Handlers, Validation)
├── Domain (Entities, ValueObject, Events)
├── Infrastructure (DbContext, DapperConnectionFactory, Repositories)
└── API (Controllers, DI-extensions)
```

---

## Поток выполнения покупки

1. Контроллер вызывает `PurchaseService`
2. `PurchaseService`:
   - запускает таймер (`IMetricService`)
   - вызывает репозиторий
   - создаёт события `ItemPurchasedEvent` для каждого товара
   - публикует их через MediatR
3. Обработчик `ItemPurchasedMetricHandler` увеличивает метрики

---

## Unit Tests
В проекте есть тесты для ```PurchaseService``` (проверка сервиса покупки и публикации событий)
```
[Fact]
public async Task Purchase_Publishes_Event_And_UsesRepo
```

## Метрики Prometheus
Метрики доступны по ```GET /metrics```

### Пример метрик в Grafana:
![Grafana](docs/metrics.jpg)

---

## Запуск проекта в Docker
docker-compose.yml включает:
- shopservice.api
- PostgreSQL с healthchecks
- Prometheus
- Grafana

Запуск ```docker compose up -d --build```

