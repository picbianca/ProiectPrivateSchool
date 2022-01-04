using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PrivateSchoolModel;
using System.Data.Entity;
using System.Data;


namespace PrivateSchool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    enum ActionState
    {
        New,
        Edit,
        Delete,
        Nothing
    }
    public partial class MainWindow : Window
    {
        ActionState action = ActionState.Nothing;
        PrivateSchoolEntitiesModel ctx = new PrivateSchoolEntitiesModel();
        CollectionViewSource profesorVSource, materieVSource, elevVSource, notaVSource;
        CollectionViewSource profesorScoalasVSource, elevCatalogsVSource;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            BindingOperations.ClearBinding(numeprofTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(prenumeprofTextBox, TextBox.TextProperty);
            SetValidationBinding();

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            BindingOperations.ClearBinding(numeprofTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(prenumeprofTextBox, TextBox.TextProperty);
            SetValidationBinding();


        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;

        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            profesorVSource.View.MoveCurrentToPrevious();

        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            profesorVSource.View.MoveCurrentToNext();

        }

        private void btnPrev1_Click(object sender, RoutedEventArgs e)
        {
            materieVSource.View.MoveCurrentToPrevious();

        }

        private void btnNext1_Click(object sender, RoutedEventArgs e)
        {
            materieVSource.View.MoveCurrentToNext();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            profesorVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("profesorViewSource")));
            profesorVSource.Source = ctx.Profesors.Local;

            materieVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("materieViewSource")));
            materieVSource.Source = ctx.Materies.Local;


            elevVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("elevViewSource")));
            elevVSource.Source = ctx.Elevs.Local;

            notaVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("notaViewSource")));
            notaVSource.Source = ctx.Notas.Local;

            elevCatalogsVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("elevCatalogsViewSource")));
            //elevCatalogsVSource.Source = ctx.Catalogs.Local;

            profesorScoalasVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("profesorScoalasViewSource")));
            //profesorScoalasVSource.Source = ctx.Scoalas.Local;

            ctx.Profesors.Load();
            ctx.Materies.Load();
            ctx.Scoalas.Load();
            ctx.Catalogs.Load();
            ctx.Elevs.Load();
            ctx.Notas.Load();

            cmbProfesor.ItemsSource = ctx.Profesors.Local;
            //cmbProfesor.DisplayMemberPath = "numeprof";
            cmbProfesor.SelectedValuePath = "ProfId";

            cmbMaterie.ItemsSource = ctx.Materies.Local;
            //cmbMaterie.DisplayMemberPath = "mdenumire";
            cmbMaterie.SelectedValuePath = "MaterieId";

            cmbElev.ItemsSource = ctx.Elevs.Local;
            //cmbElev.DisplayMemberPath = "nume";
            cmbElev.SelectedValuePath = "ElevId";

            cmbNota.ItemsSource = ctx.Notas.Local;
            //cmbNota.DisplayMemberPath = "nota";
            cmbNota.SelectedValuePath = "NotaId";


            BindDataGrid();
            
        }

        private void SaveProfesors()
        {
            Profesor profesor = null;
            if (action == ActionState.New)
            {
                try
                {

                    profesor = new Profesor()
                    {
                        numeprof = numeprofTextBox.Text.Trim(),
                        prenumeprof = prenumeprofTextBox.Text.Trim(),
                        data_angajare = data_angajareDatePicker.DisplayDate,

                    };

                    ctx.Profesors.Add(profesor);
                    profesorVSource.View.Refresh();
                    ctx.SaveChanges();
                }

                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
           if (action == ActionState.Edit)
            {
                try
                {
                    profesor = (Profesor)profesorDataGrid.SelectedItem;
                    profesor.numeprof = numeprofTextBox.Text.Trim();
                    profesor.prenumeprof = prenumeprofTextBox.Text.Trim();
                    profesor.data_angajare = data_angajareDatePicker.DisplayDate;
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    profesor = (Profesor)profesorDataGrid.SelectedItem;
                    ctx.Profesors.Remove(profesor);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                profesorVSource.View.Refresh();
            }

        }

        private void gbOperations_Click(object sender, RoutedEventArgs e)
        {

            Button SelectedButton = (Button)e.OriginalSource;
            Panel panel = (Panel)SelectedButton.Parent;

            foreach (Button B in panel.Children.OfType<Button>())
            {
                if (B != SelectedButton)
                    B.IsEnabled = false;
            }
            gbActions.IsEnabled = true;
        }
        private void ReInitialize()
        {

            Panel panel = gbOperations.Content as Panel;
            foreach (Button B in panel.Children.OfType<Button>())
            {
                B.IsEnabled = true;
            }
            gbActions.IsEnabled = false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ReInitialize();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            TabItem ti = tbCtrlPrivateSchool.SelectedItem as TabItem;

            switch (ti.Header)
            {
                case "Profesor":
                    SaveProfesors();
                    break;
                case "Materie":
                    SaveMateries();
                    break;
                case "Scoala":
                    SaveScoalas();
                    break;
                case "Elev":
                    SaveElevs();
                    break;
                case "Nota":
                    SaveNotas();
                    break;
                case "Catalog":
                    SaveCatalogs();
                    break;
            }
            ReInitialize();
        }
        private void SaveElevs()
        {
            Elev elev = null;
            if (action == ActionState.New)
            {
                try
                {

                    elev = new Elev()
                    {
                        nume = numeTextBox.Text.Trim(),
                        prenume = prenumeTextBox.Text.Trim(),

                    };

                    ctx.Elevs.Add(elev);
                    elevVSource.View.Refresh();
                    ctx.SaveChanges();
                }

                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
           if (action == ActionState.Edit)
            {
                try
                {
                    elev = (Elev)elevDataGrid.SelectedItem;
                    elev.nume = numeTextBox.Text.Trim();
                    elev.prenume = prenumeTextBox.Text.Trim();
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    elev = (Elev)elevDataGrid.SelectedItem;
                    ctx.Elevs.Remove(elev);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
             elevVSource.View.Refresh();
            }

        }
        private void SaveNotas()
        {
            Nota nota = null;
            if (action == ActionState.New)
            {
                try
                {

                    nota = new Nota()
                    {
                        nota1 = Convert.ToInt32(this.nota1TextBox.Text.Trim()),
                        detalii = detaliiTextBox.Text.Trim(),

                    };

                    ctx.Notas.Add(nota);
                    notaVSource.View.Refresh();
                    ctx.SaveChanges();
                }

                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
           if (action == ActionState.Edit)
            {
                try
                {
                    nota = (Nota)notaDataGrid.SelectedItem;
                    nota.nota1 = Convert.ToInt32(this.nota1TextBox.Text.Trim());
                    nota.detalii = detaliiTextBox.Text.Trim();
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    nota = (Nota)notaDataGrid.SelectedItem;
                    ctx.Notas.Remove(nota);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                notaVSource.View.Refresh();
            }

        }
        private void SaveMateries()
        {
            Materie materie = null;
            if (action == ActionState.New)
            {
                try
                {

                    materie = new Materie()
                    {
                        mclasa = mclasaTextBox.Text.Trim(),
                        mdenumire = mdenumireTextBox.Text.Trim(),

                    };

                    ctx.Materies.Add(materie);
                    materieVSource.View.Refresh();
                    ctx.SaveChanges();
                }

                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
           if (action == ActionState.Edit)
            {
                try
                {
                    materie = (Materie)materieDataGrid.SelectedItem;
                    materie.mclasa = mclasaTextBox.Text.Trim();
                    materie.mdenumire = mdenumireTextBox.Text.Trim();
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    materie = (Materie)materieDataGrid.SelectedItem;
                    ctx.Materies.Remove(materie);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                materieVSource.View.Refresh();
            }

        }
        private void SaveScoalas()
        {
            Scoala scoala = null;
            if (action == ActionState.New)
            {
                try
                {
                    Profesor profesor = (Profesor)cmbProfesor.SelectedItem;
                    Materie materie = (Materie)cmbMaterie.SelectedItem;

                    scoala = new Scoala()
                    {

                        ProfId = profesor.ProfId,
                        MaterieId = materie.MaterieId
                    };

                    ctx.Scoalas.Add(scoala);

                    ctx.SaveChanges();
                    BindDataGrid();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
           if (action == ActionState.Edit)
            {
                dynamic selectedScoala = scoalasDataGrid.SelectedItem;
                try
                {
                    int curr_id = selectedScoala.ScoalaId;
                    var editedScoala = ctx.Scoalas.FirstOrDefault(s => s.ScoalaId == curr_id);
                    if (editedScoala != null)
                    {
                        editedScoala.ProfId = Int32.Parse(cmbProfesor.SelectedValue.ToString());
                        editedScoala.MaterieId = Convert.ToInt32(cmbMaterie.SelectedValue.ToString());

                        ctx.SaveChanges();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                BindDataGrid();

                profesorScoalasVSource.View.MoveCurrentTo(selectedScoala);
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    dynamic selectedScoala = scoalasDataGrid.SelectedItem;
                    int curr_id = selectedScoala.ScoalaId;
                    var deletedScoala = ctx.Scoalas.FirstOrDefault(s => s.ScoalaId == curr_id);
                    if (deletedScoala != null)
                    {
                        ctx.Scoalas.Remove(deletedScoala);
                        ctx.SaveChanges();
                        MessageBox.Show("Scoala Deleted Successfully", "Message");
                        BindDataGrid();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void SaveCatalogs()
        {
            Catalog catalog = null;
            if (action == ActionState.New)
            {
                try
                {
                    Elev elev = (Elev)cmbElev.SelectedItem;
                    Nota nota = (Nota)cmbNota.SelectedItem;

                    catalog = new Catalog()
                    {

                        ElevId = elev.ElevId,
                        NotaId = nota.NotaId
                    };

                    ctx.Catalogs.Add(catalog);

                    ctx.SaveChanges();
                    BindDataGrid();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
           if (action == ActionState.Edit)
            {
                dynamic selectedCatalog = catalogsDataGrid.SelectedItem;
                try
                {
                    int curr_id = selectedCatalog.CatalogId;
                    var editedCatalog = ctx.Catalogs.FirstOrDefault(s => s.CatalogId == curr_id);
                    if (editedCatalog != null)
                    {
                        editedCatalog.ElevId = Int32.Parse(cmbElev.SelectedValue.ToString());
                        editedCatalog.NotaId = Convert.ToInt32(cmbNota.SelectedValue.ToString());

                        ctx.SaveChanges();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                BindDataGrid();

                elevCatalogsVSource.View.MoveCurrentTo(selectedCatalog);
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    dynamic selectedCatalog = catalogsDataGrid.SelectedItem;
                    int curr_id = selectedCatalog.CatalogId;
                    var deletedCatalog = ctx.Catalogs.FirstOrDefault(s => s.CatalogId == curr_id);
                    if (deletedCatalog != null)
                    {
                        ctx.Catalogs.Remove(deletedCatalog);
                        ctx.SaveChanges();
                        MessageBox.Show("Catalog Deleted Successfully", "Message");
                        BindDataGrid();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void BindDataGrid()
        {
            var queryScoala = from sc in ctx.Scoalas
                             join pf in ctx.Profesors on sc.ProfId equals
                             pf.ProfId
                             join mat in ctx.Materies on sc.MaterieId equals mat.MaterieId
                             select new
                             {
                                 sc.ScoalaId,
                                 sc.ProfId,
                                 sc.MaterieId,
                                 pf.numeprof,
                                 pf.prenumeprof,
                                 mat.mclasa,
                                 mat.mdenumire
                             };
            profesorScoalasVSource.Source = queryScoala.ToList();


            var queryCatalog = from ct in ctx.Catalogs
                             join ev in ctx.Elevs on ct.ElevId equals
                             ev.ElevId
                             join nt in ctx.Notas on ct.NotaId equals nt.NotaId
                             select new
                             {
                                 ct.CatalogId,
                                 ct.ElevId,
                                 ct.NotaId,
                                 ev.nume,
                                 ev.prenume,
                                 nt.nota1,
                                 nt.detalii
                             };
            elevCatalogsVSource.Source = queryCatalog.ToList();
        }
        private void SetValidationBinding()
        {
            Binding numeprofValidationBinding = new Binding();
            numeprofValidationBinding.Source = profesorVSource;
            numeprofValidationBinding.Path = new PropertyPath("numeprof");
            numeprofValidationBinding.NotifyOnValidationError = true;
            numeprofValidationBinding.Mode = BindingMode.TwoWay;
            numeprofValidationBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            numeprofValidationBinding.ValidationRules.Add(new StringNotEmpty());
            numeprofTextBox.SetBinding(TextBox.TextProperty,numeprofValidationBinding);

            Binding prenumeprofValidationBinding = new Binding();
            prenumeprofValidationBinding.Source = profesorVSource;
            prenumeprofValidationBinding.Path = new PropertyPath("prenumeprof");
            prenumeprofValidationBinding.NotifyOnValidationError = true;
            prenumeprofValidationBinding.Mode = BindingMode.TwoWay;
            prenumeprofValidationBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            prenumeprofValidationBinding.ValidationRules.Add(new
           StringMinLengthValidator());
            prenumeprofTextBox.SetBinding(TextBox.TextProperty,prenumeprofValidationBinding); 
        }
    }

}
