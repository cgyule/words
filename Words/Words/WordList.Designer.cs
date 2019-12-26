namespace Words
{
    partial class WordList

    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.numMutations = new System.Windows.Forms.NumericUpDown();
            this.chkAnagram = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.lbWords = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeLettersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyAllToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button2 = new System.Windows.Forms.Button();
            this.chkSubst = new System.Windows.Forms.CheckBox();
            this.chkInsertion = new System.Windows.Forms.CheckBox();
            this.chkDeletion = new System.Windows.Forms.CheckBox();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.lbUrl = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblEntries = new System.Windows.Forms.Label();
            this.chkDoAll = new System.Windows.Forms.CheckBox();
            this.cbLetters = new System.Windows.Forms.ComboBox();
            this.cbPattern = new System.Windows.Forms.ComboBox();
            this.chkUseDiacritic = new System.Windows.Forms.CheckBox();
            this.chkBeg = new System.Windows.Forms.CheckBox();
            this.chkEnd = new System.Windows.Forms.CheckBox();
            this.numWords = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker3 = new System.ComponentModel.BackgroundWorker();
            this.label4 = new System.Windows.Forms.Label();
            this.numMinLength = new System.Windows.Forms.NumericUpDown();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripWordLists = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.btnRefreshWordList = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripDigram = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripHistory = new System.Windows.Forms.ToolStripComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numMutations)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWords)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinLength)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // numMutations
            // 
            this.numMutations.Location = new System.Drawing.Point(78, 128);
            this.numMutations.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numMutations.Name = "numMutations";
            this.numMutations.Size = new System.Drawing.Size(38, 20);
            this.numMutations.TabIndex = 3;
            this.toolTip1.SetToolTip(this.numMutations, "Total number of substitutions/insertions and deletions");
            // 
            // chkAnagram
            // 
            this.chkAnagram.AutoSize = true;
            this.chkAnagram.Location = new System.Drawing.Point(22, 200);
            this.chkAnagram.Name = "chkAnagram";
            this.chkAnagram.Size = new System.Drawing.Size(68, 17);
            this.chkAnagram.TabIndex = 5;
            this.chkAnagram.Text = "Anagram";
            this.toolTip1.SetToolTip(this.chkAnagram, "Find anagrams of source letters");
            this.chkAnagram.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Letters:";
            this.label1.UseWaitCursor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Mutations:";
            this.label2.UseWaitCursor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Pattern:";
            this.label5.UseWaitCursor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // lbWords
            // 
            this.lbWords.ContextMenuStrip = this.contextMenuStrip1;
            this.lbWords.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbWords.FormattingEnabled = true;
            this.lbWords.HorizontalScrollbar = true;
            this.lbWords.ItemHeight = 20;
            this.lbWords.Location = new System.Drawing.Point(109, 200);
            this.lbWords.Name = "lbWords";
            this.lbWords.Size = new System.Drawing.Size(232, 404);
            this.lbWords.TabIndex = 14;
            this.lbWords.TabStop = false;
            this.lbWords.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbWords_MouseDoubleClick);
            this.lbWords.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbWords_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeLettersToolStripMenuItem,
            this.copyToClipboardToolStripMenuItem,
            this.copyAllToClipboardToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(185, 70);
            // 
            // removeLettersToolStripMenuItem
            // 
            this.removeLettersToolStripMenuItem.Name = "removeLettersToolStripMenuItem";
            this.removeLettersToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.removeLettersToolStripMenuItem.Text = "Remove Letters";
            this.removeLettersToolStripMenuItem.Click += new System.EventHandler(this.removeLettersToolStripMenuItem_Click);
            // 
            // copyToClipboardToolStripMenuItem
            // 
            this.copyToClipboardToolStripMenuItem.Name = "copyToClipboardToolStripMenuItem";
            this.copyToClipboardToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.copyToClipboardToolStripMenuItem.Text = "Copy to clipboard";
            this.copyToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyToClipboardToolStripMenuItem_Click);
            // 
            // copyAllToClipboardToolStripMenuItem
            // 
            this.copyAllToClipboardToolStripMenuItem.Name = "copyAllToClipboardToolStripMenuItem";
            this.copyAllToClipboardToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.copyAllToClipboardToolStripMenuItem.Text = "Copy all to clipboard";
            this.copyAllToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyAllToClipboardToolStripMenuItem_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(18, 342);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(43, 36);
            this.button2.TabIndex = 9;
            this.button2.Text = "Go";
            this.toolTip1.SetToolTip(this.button2, "Perform search");
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // chkSubst
            // 
            this.chkSubst.AutoSize = true;
            this.chkSubst.Location = new System.Drawing.Point(22, 232);
            this.chkSubst.Name = "chkSubst";
            this.chkSubst.Size = new System.Drawing.Size(81, 17);
            this.chkSubst.TabIndex = 6;
            this.chkSubst.Text = "Substitution";
            this.toolTip1.SetToolTip(this.chkSubst, "Substitute - mutation to substitute letters");
            this.chkSubst.UseVisualStyleBackColor = true;
            // 
            // chkInsertion
            // 
            this.chkInsertion.AutoSize = true;
            this.chkInsertion.Location = new System.Drawing.Point(22, 264);
            this.chkInsertion.Name = "chkInsertion";
            this.chkInsertion.Size = new System.Drawing.Size(66, 17);
            this.chkInsertion.TabIndex = 7;
            this.chkInsertion.Text = "Insertion";
            this.toolTip1.SetToolTip(this.chkInsertion, "Mutation to insert letters");
            this.chkInsertion.UseVisualStyleBackColor = true;
            // 
            // chkDeletion
            // 
            this.chkDeletion.AutoSize = true;
            this.chkDeletion.Location = new System.Drawing.Point(22, 296);
            this.chkDeletion.Name = "chkDeletion";
            this.chkDeletion.Size = new System.Drawing.Size(65, 17);
            this.chkDeletion.TabIndex = 8;
            this.chkDeletion.Text = "Deletion";
            this.toolTip1.SetToolTip(this.chkDeletion, "Mutation to delete letters");
            this.chkDeletion.UseVisualStyleBackColor = true;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(352, 32);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(1079, 781);
            this.webBrowser1.TabIndex = 19;
            this.webBrowser1.TabStop = false;
            this.webBrowser1.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowser1_Navigated);
            // 
            // lbUrl
            // 
            this.lbUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbUrl.FormattingEnabled = true;
            this.lbUrl.Location = new System.Drawing.Point(13, 640);
            this.lbUrl.Name = "lbUrl";
            this.lbUrl.Size = new System.Drawing.Size(328, 173);
            this.lbUrl.TabIndex = 20;
            this.lbUrl.TabStop = false;
            this.lbUrl.SelectedIndexChanged += new System.EventHandler(this.lbUrl_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 616);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(250, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "URL pattern (selected word will substitute %word% )";
            this.label3.UseWaitCursor = true;
            // 
            // lblEntries
            // 
            this.lblEntries.AutoSize = true;
            this.lblEntries.Location = new System.Drawing.Point(101, 179);
            this.lblEntries.Name = "lblEntries";
            this.lblEntries.Size = new System.Drawing.Size(72, 13);
            this.lblEntries.TabIndex = 22;
            this.lblEntries.Text = "Entries found:";
            this.lblEntries.UseWaitCursor = true;
            // 
            // chkDoAll
            // 
            this.chkDoAll.AutoSize = true;
            this.chkDoAll.Location = new System.Drawing.Point(131, 130);
            this.chkDoAll.Name = "chkDoAll";
            this.chkDoAll.Size = new System.Drawing.Size(53, 17);
            this.chkDoAll.TabIndex = 4;
            this.chkDoAll.Text = "Do all";
            this.toolTip1.SetToolTip(this.chkDoAll, "Do all mutations up to selected number");
            this.chkDoAll.UseVisualStyleBackColor = true;
            // 
            // cbLetters
            // 
            this.cbLetters.AllowDrop = true;
            this.cbLetters.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.cbLetters.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbLetters.FormattingEnabled = true;
            this.cbLetters.Location = new System.Drawing.Point(78, 54);
            this.cbLetters.Name = "cbLetters";
            this.cbLetters.Size = new System.Drawing.Size(229, 28);
            this.cbLetters.TabIndex = 1;
            this.toolTip1.SetToolTip(this.cbLetters, "Source letters");
            this.cbLetters.TextChanged += new System.EventHandler(this.cbLetters_TextChanged);
            this.cbLetters.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbLetters_KeyPress);
            // 
            // cbPattern
            // 
            this.cbPattern.AllowDrop = true;
            this.cbPattern.CausesValidation = false;
            this.cbPattern.Cursor = System.Windows.Forms.Cursors.Default;
            this.cbPattern.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbPattern.FormattingEnabled = true;
            this.cbPattern.Location = new System.Drawing.Point(78, 91);
            this.cbPattern.Name = "cbPattern";
            this.cbPattern.Size = new System.Drawing.Size(229, 28);
            this.cbPattern.TabIndex = 2;
            this.toolTip1.SetToolTip(this.cbPattern, "Regular expression to filter wordlist");
            this.cbPattern.TextChanged += new System.EventHandler(this.cbPattern_TextChanged);
            this.cbPattern.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbPattern_KeyPress);
            // 
            // chkUseDiacritic
            // 
            this.chkUseDiacritic.AutoSize = true;
            this.chkUseDiacritic.Cursor = System.Windows.Forms.Cursors.Default;
            this.chkUseDiacritic.Location = new System.Drawing.Point(17, 32);
            this.chkUseDiacritic.Name = "chkUseDiacritic";
            this.chkUseDiacritic.Size = new System.Drawing.Size(271, 17);
            this.chkUseDiacritic.TabIndex = 28;
            this.chkUseDiacritic.TabStop = false;
            this.chkUseDiacritic.Text = "Use diacritics (causes reload of word list on change)";
            this.toolTip1.SetToolTip(this.chkUseDiacritic, "Tick to use accented characters.");
            this.chkUseDiacritic.UseVisualStyleBackColor = true;
            this.chkUseDiacritic.CheckedChanged += new System.EventHandler(this.chkUseDiacritic_CheckedChanged);
            // 
            // chkBeg
            // 
            this.chkBeg.AutoSize = true;
            this.chkBeg.Location = new System.Drawing.Point(314, 90);
            this.chkBeg.Name = "chkBeg";
            this.chkBeg.Size = new System.Drawing.Size(32, 17);
            this.chkBeg.TabIndex = 30;
            this.chkBeg.Text = "^";
            this.toolTip1.SetToolTip(this.chkBeg, "Match word start");
            this.chkBeg.UseVisualStyleBackColor = true;
            // 
            // chkEnd
            // 
            this.chkEnd.AutoSize = true;
            this.chkEnd.Location = new System.Drawing.Point(314, 106);
            this.chkEnd.Name = "chkEnd";
            this.chkEnd.Size = new System.Drawing.Size(32, 17);
            this.chkEnd.TabIndex = 31;
            this.chkEnd.Text = "$";
            this.toolTip1.SetToolTip(this.chkEnd, "Match word end");
            this.chkEnd.UseVisualStyleBackColor = true;
            // 
            // numWords
            // 
            this.numWords.Location = new System.Drawing.Point(300, 128);
            this.numWords.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.numWords.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numWords.Name = "numWords";
            this.numWords.Size = new System.Drawing.Size(38, 20);
            this.numWords.TabIndex = 32;
            this.toolTip1.SetToolTip(this.numWords, "Generate word combinations from all lsource letters");
            this.numWords.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numWords.ValueChanged += new System.EventHandler(this.numWords_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(236, 130);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 33;
            this.label8.Text = "Multi word:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(236, 156);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 34;
            this.label4.Text = "Min Length:";
            // 
            // numMinLength
            // 
            this.numMinLength.Location = new System.Drawing.Point(300, 154);
            this.numMinLength.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.numMinLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMinLength.Name = "numMinLength";
            this.numMinLength.Size = new System.Drawing.Size(38, 20);
            this.numMinLength.TabIndex = 35;
            this.numMinLength.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.toolStripWordLists,
            this.toolStripLabel1,
            this.btnRefreshWordList,
            this.toolStripSeparator1,
            this.toolStripLabel3,
            this.toolStripDigram,
            this.toolStripSeparator2,
            this.toolStripLabel4,
            this.toolStripHistory});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1446, 28);
            this.toolStrip1.TabIndex = 36;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(74, 24);
            this.toolStripLabel2.Text = "Word list:";
            // 
            // toolStripWordLists
            // 
            this.toolStripWordLists.BackColor = System.Drawing.Color.LightYellow;
            this.toolStripWordLists.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripWordLists.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripWordLists.Name = "toolStripWordLists";
            this.toolStripWordLists.Size = new System.Drawing.Size(230, 28);
            this.toolStripWordLists.Sorted = true;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(0, 24);
            // 
            // btnRefreshWordList
            // 
            this.btnRefreshWordList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnRefreshWordList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefreshWordList.Name = "btnRefreshWordList";
            this.btnRefreshWordList.RightToLeftAutoMirrorImage = true;
            this.btnRefreshWordList.Size = new System.Drawing.Size(70, 24);
            this.btnRefreshWordList.Text = "Refresh";
            this.btnRefreshWordList.Click += new System.EventHandler(this.button1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(98, 24);
            this.toolStripLabel3.Text = "Digram filter:";
            // 
            // toolStripDigram
            // 
            this.toolStripDigram.BackColor = System.Drawing.Color.LightYellow;
            this.toolStripDigram.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripDigram.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripDigram.Name = "toolStripDigram";
            this.toolStripDigram.Size = new System.Drawing.Size(121, 28);
            this.toolStripDigram.Sorted = true;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(46, 24);
            this.toolStripLabel4.Text = "URL:";
            // 
            // toolStripHistory
            // 
            this.toolStripHistory.BackColor = System.Drawing.Color.LightYellow;
            this.toolStripHistory.DropDownWidth = 820;
            this.toolStripHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripHistory.Name = "toolStripHistory";
            this.toolStripHistory.Size = new System.Drawing.Size(720, 28);
            this.toolStripHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripHistory_KeyPress);
            // 
            // WordList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1446, 822);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.numMinLength);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.numWords);
            this.Controls.Add(this.chkEnd);
            this.Controls.Add(this.chkBeg);
            this.Controls.Add(this.chkUseDiacritic);
            this.Controls.Add(this.cbPattern);
            this.Controls.Add(this.cbLetters);
            this.Controls.Add(this.chkDoAll);
            this.Controls.Add(this.lblEntries);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbUrl);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.chkDeletion);
            this.Controls.Add(this.chkInsertion);
            this.Controls.Add(this.chkSubst);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.lbWords);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkAnagram);
            this.Controls.Add(this.numMutations);
            this.Name = "WordList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fuzzy Word";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WordList_FormClosing);
            this.Load += new System.EventHandler(this.WordList_Load);
            this.Shown += new System.EventHandler(this.WordList_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.numMutations)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numWords)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinLength)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown numMutations;
        private System.Windows.Forms.CheckBox chkAnagram;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ListBox lbWords;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox chkSubst;
        private System.Windows.Forms.CheckBox chkInsertion;
        private System.Windows.Forms.CheckBox chkDeletion;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.ListBox lbUrl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label lblEntries;
        private System.Windows.Forms.ToolStripMenuItem copyToClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyAllToClipboardToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkDoAll;
        private System.Windows.Forms.ComboBox cbLetters;
        private System.Windows.Forms.ComboBox cbPattern;
        private System.Windows.Forms.CheckBox chkUseDiacritic;
        private System.Windows.Forms.CheckBox chkBeg;
        private System.Windows.Forms.CheckBox chkEnd;
        private System.Windows.Forms.ToolStripMenuItem removeLettersToolStripMenuItem;
        private System.Windows.Forms.NumericUpDown numWords;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.ComponentModel.BackgroundWorker backgroundWorker3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numMinLength;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox toolStripWordLists;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripButton btnRefreshWordList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox toolStripDigram;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripComboBox toolStripHistory;
    }
}

