namespace libermedical.Pages
{
	public partial class PlusPage
    {
        public PlusPage() : base(0, 0)
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            bt.TranslationY = -30;

        }
    }
}
