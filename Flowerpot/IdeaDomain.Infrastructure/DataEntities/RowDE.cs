
namespace IdeaDomain.InfrastructureLayer.DataEntities
{
    public class RowDE
    {
        public RowDE()
        {
            Version = 1;
            IsDeleted = false;
        }

        public int RowId { get; set; }

        public int IdeaId { get; set; }

        public int Version { get; set; }

        public bool IsDeleted { get; set; }

    }
}
