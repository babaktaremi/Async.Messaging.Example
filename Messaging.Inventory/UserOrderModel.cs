namespace Messaging.Inventory;

public record UserOrderModel(int UserId, int ProductId, string ProductName, int Amount);