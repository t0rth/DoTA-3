namespace DoTA_3
{
    public class CCountdownTimer
    {
        private double targetTime;
        private double currentTime;

        public CCountdownTimer(double timeBeforeClick)
        {
            targetTime = timeBeforeClick;
            currentTime = 0;
        }

        public bool IsReady => currentTime >= targetTime;

        public void Update(double delta)
        {
            if (currentTime < targetTime)
                currentTime += delta;
        }

        public void Reset()
        {
            currentTime = 0;
        }

        public void DecreaseTargetTime(double amount)
        {
            targetTime -= amount;
            if (targetTime < 0.1) targetTime = 0.1;
        }
    }
}