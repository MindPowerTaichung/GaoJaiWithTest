namespace MPERP2015.Reports
{
    partial class CustomerLabelReport
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            this.objectDataSource1 = new Telerik.Reporting.ObjectDataSource();
            this.detail = new Telerik.Reporting.DetailSection();
            this.nameDataTextBox = new Telerik.Reporting.TextBox();
            this.shipaddrDataTextBox = new Telerik.Reporting.TextBox();
            this.telephone1DataTextBox = new Telerik.Reporting.TextBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(MPERP2015.Models.CustomerViewModel);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Mm(36D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.nameDataTextBox,
            this.shipaddrDataTextBox,
            this.telephone1DataTextBox,
            this.textBox1});
            this.detail.Name = "detail";
            // 
            // nameDataTextBox
            // 
            this.nameDataTextBox.CanGrow = false;
            this.nameDataTextBox.CanShrink = false;
            this.nameDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.39989966154098511D), Telerik.Reporting.Drawing.Unit.Cm(0.30000004172325134D));
            this.nameDataTextBox.Name = "nameDataTextBox";
            this.nameDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(10.199999809265137D), Telerik.Reporting.Drawing.Unit.Cm(1.8998004198074341D));
            this.nameDataTextBox.Style.Font.Name = "Microsoft YaHei";
            this.nameDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15D);
            this.nameDataTextBox.Value = "= Fields.Shipaddr";
            // 
            // shipaddrDataTextBox
            // 
            this.shipaddrDataTextBox.CanGrow = false;
            this.shipaddrDataTextBox.CanShrink = false;
            this.shipaddrDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.39989966154098511D), Telerik.Reporting.Drawing.Unit.Cm(2.2000002861022949D));
            this.shipaddrDataTextBox.Name = "shipaddrDataTextBox";
            this.shipaddrDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(10.199999809265137D), Telerik.Reporting.Drawing.Unit.Cm(0.69980019330978394D));
            this.shipaddrDataTextBox.Style.Font.Name = "Microsoft YaHei";
            this.shipaddrDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(14D);
            this.shipaddrDataTextBox.Value = "= Fields.Name";
            // 
            // telephone1DataTextBox
            // 
            this.telephone1DataTextBox.CanGrow = false;
            this.telephone1DataTextBox.CanShrink = false;
            this.telephone1DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.39989966154098511D), Telerik.Reporting.Drawing.Unit.Cm(2.9000003337860107D));
            this.telephone1DataTextBox.Name = "telephone1DataTextBox";
            this.telephone1DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.6001005172729492D), Telerik.Reporting.Drawing.Unit.Cm(0.39999982714653015D));
            this.telephone1DataTextBox.Style.Font.Name = "Microsoft YaHei";
            this.telephone1DataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.telephone1DataTextBox.Value = "= Fields.Telephone1";
            // 
            // textBox1
            // 
            this.textBox1.CanGrow = false;
            this.textBox1.CanShrink = false;
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.3999996185302734D), Telerik.Reporting.Drawing.Unit.Cm(2.900001049041748D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.6001005172729492D), Telerik.Reporting.Drawing.Unit.Cm(0.39999982714653015D));
            this.textBox1.Style.Font.Name = "Microsoft YaHei";
            this.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.textBox1.Value = "= Fields.Telephone3";
            // 
            // CustomerLabelReport
            // 
            this.DataSource = this.objectDataSource1;
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.detail});
            this.Name = "CustomerLabelReport";
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(0D), Telerik.Reporting.Drawing.Unit.Mm(0D), Telerik.Reporting.Drawing.Unit.Mm(0D), Telerik.Reporting.Drawing.Unit.Mm(0D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.PageSettings.PaperSize = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(110D), Telerik.Reporting.Drawing.Unit.Mm(36D));
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1});
            this.Width = Telerik.Reporting.Drawing.Unit.Mm(110D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.ObjectDataSource objectDataSource1;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.TextBox nameDataTextBox;
        private Telerik.Reporting.TextBox shipaddrDataTextBox;
        private Telerik.Reporting.TextBox telephone1DataTextBox;
        private Telerik.Reporting.TextBox textBox1;

    }
}