using libermedical.CustomControls;

namespace libermedical.Pages
{
    public partial class OrdonnanceFrequence2Page : BasePage
    {

        public OrdonnanceFrequence2Page() : base(-1, 64, false)
        {
            BindingContext = this;
            InitializeComponent();
        }
        async void Cancel_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        async void Save_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();

        }
        async void Cotations_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new OrdonnanceCotationPage());
        }

        void Majoration_Tapped(object sender, System.EventArgs e)
        {

            //Changement des majorations à la tap sur la zone
            string[] tab = { "Non", "MAU", "MCI" };
            var cot = maj.Text;

            for (var i = 0; i < tab.Length; i++)
            {
                if ((cot == tab[i]) && (i < tab.Length - 1)) { maj.Text = tab[i + 1]; }
                if ((cot == tab[i]) && (i == tab.Length - 1)) { maj.Text = tab[0]; }

            }


        }

        void Deplacement_Tapped(object sender, System.EventArgs e)
        {

        }
    }
}
