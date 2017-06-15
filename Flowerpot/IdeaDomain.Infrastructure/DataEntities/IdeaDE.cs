using System;

namespace IdeaDomain.InfrastructureLayer.DataEntities
{
    public class IdeaDE
    {
        public IdeaDE()
        {
            CreateTime = DateTime.Now;
            IsDeleted = false;
        }

        public int IdeaId { get; set; }

        public string IdeaName { get; set; }

        public string IdeaDescription { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsDeleted { get; set; }

        public int UserId { get; set; }

    }
}
