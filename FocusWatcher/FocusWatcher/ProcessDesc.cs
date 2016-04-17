using System;

namespace FocusWatcher
{
    class ProcessDesc
    {
        public DateTime StartTime;
        public DateTime EndTime;
        public string Name { get; set; }
        public string Title { get; set; }
        public TimeSpan Duration { get { return EndTime.Subtract(StartTime); } }
        public ProcessDesc() { }

        static public ProcessDesc LockedSession
        {
            get
            {
                return new ProcessDesc()
                {
                    StartTime = DateTime.Now,
                    Name = "LockedSession",
                    Title="Locked"
                };
            }
        }
        new public string ToString()
        {
            return string.Format("{0};{1};{2}", this.Duration, this.Name, this.Title);
        }
    }
}
