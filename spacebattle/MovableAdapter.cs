namespace spacebattle
{
    internal class MovableAdapter : IMovable
    {
        private readonly UObject _obj;
        public MovableAdapter(UObject obj)
        {
            _obj = obj;
        }

        public Vector Position { get => (Vector)_obj.args.GetP("Position"); set => _obj.args.Add("Position", value); }

        public Vector Velocity => (Vector)_obj.args.GetP("Velocity");
    }
}
