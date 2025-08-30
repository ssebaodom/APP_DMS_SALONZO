using SSE.Common.Api.v1.Common;
using SSE.Common.DTO.v1;

namespace SSE.Common.Api.v1.Requests.Order
{
    public class OrderCreateRequest : CommonRequest
    {
        public OrderCreateDTO Data { get; set; }
    }
    
    public class OrderCreateV3Request : CommonRequest
    {
        public OrderCreateV3DTO Data { get; set; }
    }

    public class RefundOrderCreateV1Request : CommonRequest
    {
        public RefundOrderCreateV1DTO Data { get; set; }
    }

    public class OrderUpdateRequest : CommonRequest
    {
        public OrderUpdateDTO Data { get; set; }
    }
    public class UpdateQuantityWarehouseDeliveryRequest : CommonRequest
    {
        public UpdateQuantityWarehouseDeliveryDTO Data { get; set; }
    }
    public class UpdateQuantityPostPNFRequest : CommonRequest
    {
        public UpdateQuantityPostPNFDTO Data { get; set; }
    }
    public class CreateDeliveryRequest : CommonRequest
    {
        public CreateDeliveryCardDTO Data { get; set; }
    }
    public class UpdateItemBarcodeRequest : CommonRequest
    {
        public UpdateItemBarcodeDTO Data { get; set; }
    }
    public class ItemLocaionModifyRequest : CommonRequest
    {
        public ItemLocaionModifyDTO Data { get; set; }
    }
    public class CreateItemHolderRequest : CommonRequest
    {
        public CreateItemHolderDTO Data { get; set; }
    }
}