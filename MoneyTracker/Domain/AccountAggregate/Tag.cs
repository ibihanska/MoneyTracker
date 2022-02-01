namespace MoneyTracker.Domain.AccountAggregate
{
    public class Tag
    {
        public Tag(string name)
        {
            Name = name;
        }
        public string Name { get; private set; }
        public override string ToString() => Name;
        public override int GetHashCode() => Name.GetHashCode();
        public override bool Equals(object obj)
        {
            bool check = false;
            if (obj != null && obj is Tag)
            {
                Tag tag2 = (Tag)obj;
                if (Name == tag2.Name)
                {
                    check = true;
                }
                else
                {
                    return check;
                }
            }
            return check;
        }
        public static bool operator ==(Tag tag1, Tag tag2) => tag1.Equals(tag2);
        public static bool operator !=(Tag tag1, Tag tag2) => !tag1.Equals(tag2);
    }
}
