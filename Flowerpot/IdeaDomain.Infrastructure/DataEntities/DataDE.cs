
namespace IdeaDomain.InfrastructureLayer.DataEntities
{
    public class DataDE
    {
        public DataDE()
        {
            IsDeleted = false;
        }

        public object Value { get; set; }

        public int ColumnId { get; set; }

        public int RowId { get; set; }

        public bool IsDeleted { get; set; }

    }
}
