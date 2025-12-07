namespace ShopService.Application.Services.Interfaces;

public interface IMetricService
{
    void RecordItemPurchased();
    void RecordItemCreated();
    IDisposable BeginPurchaseTimer();
}