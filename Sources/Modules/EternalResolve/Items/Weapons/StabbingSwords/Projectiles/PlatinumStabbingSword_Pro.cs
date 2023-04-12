namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class PlatinumStabbingSword_Pro : StabbingProjectile
    {
        public override void SetDefaults()
        {
            Color = new Color(166, 166, 226);
            base.SetDefaults();
			TradeLength = 4;
			TradeShade = 0.7f;
			Shade = 0.2f;
			FadeTradeShade = 0.64f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.4f;
			MaxLength = 0.90f;
			DrawWidth = 0.4f;
		}
    }
}