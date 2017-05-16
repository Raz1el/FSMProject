namespace FSMLibrary.NFSMBuild
{
    class Id
    {
        private static int _currentId;
        public static int GetId()
        {
            return ++_currentId;
        }
    }
}
