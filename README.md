# ShopService — сервис покупок с метриками Prometheus и мониторингом Grafana

Этот проект реализует упрощённый backend-магазин с возможностью создания товаров, совершения покупок, публикацией доменных событий и системой сбора метрик.  
Сервис построен по принципам **DDD**, **Clean Architecture**, использует **MediatR** для событий, **Prometheus** для метрик и **Grafana** для визуализации.

---

## Технологии
- **.NET**
- **ASP.NET Core WebAPI**
- **Entity Framework Core**
- **Dapper**
- **MediatR** — доменные события
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

