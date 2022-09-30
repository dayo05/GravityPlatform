namespace GravityPlatform.Scenes.Maps.Tutorial
{
    public class TutorialPlayer : Player.Player
    {
        public override void _Ready()
        {
            Connect("OnClear", this, nameof(SOnClear2));
            base._Ready();
        }

        private void SOnClear2()
        {
            InternalData.isTutorialCleared = true;
        }
    }
}