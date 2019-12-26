using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Collections;
using Microsoft.Win32;

/// <summary>
/// Word matching toolbox. All words or anagrams within a specified Shannon edit distance can be found. Edit operations consist
/// of a user specified combination of insertions, substitutions and deletions. 
/// The edit distance is the total number of operations/mutations carried out on the input string.
/// The found matches can be filtered using a regular expression.
/// Double clicking on a found word will use a url to perform a lookup on the web.
/// 
/// Author Charles Yule 01/03/2018
/// 
/// </summary>
namespace Words
{
    public partial class WordList : Form
    {
        //filter for short words for use with the multi word finder (otherwise all sorts of garbage/acronyms can be found which makes it somewhat unmanageable).
        SortedSet<string> let2 = new SortedSet<string> { "a", "i", "o", "aa", "ad", "an", "am", "at", "ah", "al", "as", "ay", "by", "be", "do", "ee", "ex", "et", "go", "ho", "hi", "he", "ha", "is", "it", "in", "if", "la", "le", "lo", "me", "ma", "my", "mo", "no", "of", "on", "oh", "or", "ow", "ox", "oy", "oi", "oz", "pa", "po", "pi", "so", "to", "ta", "up", "we" };
        string wordFile = "";
        string digramFile = "";
        string wordFileName = "UKACD17.txt";
        string digramFileName = "english_filt.txt";
        string[] words = null;
        string dataPath = "";
        string listPath = "";
        string digramPath = "";
        string lastURL = "";
        int numEntries = 0;
        int countEntries = 0;
        string gWordIn = "";
        bool gChkSubst = false;
        bool gChkDeletion = false;
        bool gChkInsertion = false;
        bool gChkAnagram = false;
        bool gChkUseDiacritic = false;
        bool gChkDoAll = false;
        int gNumMutations = 0;
        int gNumWords = 0;
        int gNumMinLength = 0;


        FormProg pbar = new FormProg();

        int maxHistory = 10;
        Regex re = null;
        //char[] vowels = { 'a', 'e', 'i', 'o', 'u', 'y' };
        string filter = "";
        treeElement wordTree = null;
        treeElement wordTreeSorted = null;
        SortedSet<string> multiWords = null;
        SortedSet<string> foundWords = null;
        string currentLetters = "";

        List<List<char>> inLetters;
        //global for number of words to find
        int numberOfWords = 1;

        public WordList()
        {
            InitializeComponent();
//set browser compatibiliuty
            int BrowserVer, RegVal;

            // get the installed IE version
            using (WebBrowser Wb = new WebBrowser())
                BrowserVer = Wb.Version.Major;

            // set the appropriate IE version
            if (BrowserVer >= 11)
                RegVal = 11001;
            else if (BrowserVer == 10)
                RegVal = 10001;
            else if (BrowserVer == 9)
                RegVal = 9999;
            else if (BrowserVer == 8)
                RegVal = 8888;
            else
                RegVal = 7000;

            // set the actual key
            using (RegistryKey Key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", RegistryKeyPermissionCheck.ReadWriteSubTree))
                if (Key.GetValue(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe") == null)
                    Key.SetValue(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe", RegVal, RegistryValueKind.DWord);
//set up background processes
            //wordlist loading
            backgroundWorker1.DoWork += backgroundWorker1_buildWordTree;
            backgroundWorker1.ProgressChanged += BackgroundWorker1_ProgressChanged;
            backgroundWorker1.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = false;
            //searching
            backgroundWorker2.DoWork += doSearch;
            backgroundWorker2.ProgressChanged += BackgroundWorker2_ProgressChanged;
            backgroundWorker2.RunWorkerCompleted += BackgroundWorker2_RunWorkerCompleted;
            backgroundWorker2.WorkerReportsProgress = true;
            backgroundWorker2.WorkerSupportsCancellation = true;

            //declare delegates for toolstrip combo boxes
            toolStripHistory.ComboBox.SelectionChangeCommitted += toolStripHistory_SelectedChangeCommitted;
            toolStripDigram.ComboBox.SelectionChangeCommitted += toolStripDigram_SelectedChangeCommitted;
            toolStripWordLists.ComboBox.SelectionChangeCommitted += toolStripWordLists_SelectedChangeCommitted;

            //set file locations
            dataPath = Application.StartupPath + "\\data\\";
            listPath = dataPath + "lists\\";
            digramPath = dataPath + "lists\\digram_filters\\";
            string defpath = dataPath + "defaults.txt";
            string urllist = dataPath + "urls.txt";
            inLetters = new List<List<char>>();
            string defURL = "";
            chkBeg.Checked = true;
            chkEnd.Checked = true;
            //read defaults
            if (File.Exists(defpath))
            {
                string[] defaults = File.ReadAllLines(defpath);
                foreach (string s in defaults)
                {
                    string strimmed = s.Trim(' ');
                    int idx = strimmed.IndexOf('=');
                    if (idx > 0 && idx < strimmed.Length - 1)
                    {
                        string sleft = strimmed.Substring(0, idx).ToLower();
                        string sright = strimmed.Substring(idx + 1);
                        if (sleft == "wordlist")
                        {
                            wordFileName = sright;
                        }
                        if (sleft == "digrams")
                        {
                            digramFileName = sright;
                        }
                        else if (sleft == "url")
                        {
                            defURL = sright;
                        }
                        else if (sleft == "maxhistory")
                        {
                            if (!int.TryParse(sright, out maxHistory))
                            {
                                maxHistory = 10;
                            }
                        }
                    }

                }

            }
            restoreSession();
            wordFile = wordFileName;
            digramFile = digramFileName;
            // %word% is the placeholder for the word to look up. Not interested in urls that don't contain this pattern.
            if (lastURL != "" && lastURL.ToLower().Contains("%word%"))
            {
                defURL = lastURL;
            }
            //add url patterns to listbox
            int idxurl = -1;
            if (File.Exists(urllist))
            {
                string[] urls = File.ReadAllLines(urllist);
                foreach (string u in urls)
                {
                    if (u.ToLower().Contains("%word%"))
                    {
                        lbUrl.Items.Add(u.Trim(' '));
                        if (defURL != "")
                        {
                            if (u.ToLower() == defURL.ToLower())
                            {
                                idxurl = lbUrl.Items.Count - 1;
                            }
                        }
                    }
                }
                if (idxurl < 0 && defURL.ToLower().Contains("%word%"))
                {
                    idxurl = lbUrl.Items.Add(defURL.Trim(' '));
                }
                if (idxurl > -1)
                {
                    lbUrl.SelectedIndex = idxurl;
                }

            }
            cbWordListsRefresh(true);
            readDigrams();

            //load initial word list (if it exists)
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pbar.Close();
        }

        private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbar.setProgress(e.ProgressPercentage, countEntries.ToString() + " of " + numEntries.ToString() + " loaded.");
        }
        private void BackgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pbar.Close();
        }

        private void BackgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbar.setProgress(e.ProgressPercentage, foundWords.Count.ToString() + " entries found");
        }

        /// <summary>
        /// Determines a text file's encoding by analyzing its byte order mark (BOM).
        /// Defaults to ASCII when detection of the text file's endianness fails.
        /// </summary>
        /// <param name="filename">The text file to analyze.</param>
        /// <returns>The detected encoding.</returns>
        public static Encoding GetEncoding(string filename)
        {
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.Read(bom, 0, 4);
            }

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;
            return Encoding.UTF7;
        }
        /// <summary>
        /// Attempt to get file encoding
        /// Not sure at the moment how unicode will work, maybe fine, maybe not!
        /// </summary>
        /// <param name="filePath">The file to examine</param>
        /// <returns></returns>
        public Encoding GetFileEncoding(string filePath)
        {
            Encoding ret = Encoding.UTF8;
            using (StreamReader sr = new StreamReader(filePath, true))
            {
                sr.Read();
                ret = sr.CurrentEncoding;
            }
            return ret;
        }
        /// <summary>
        /// Reads a word list file
        /// </summary>
        /// <returns>Returns an array of words</returns>
        private string[] readWords()
        {
            string[] ret = null;
            string wf = listPath + wordFile;
            try
            {
                if (File.Exists(wf))
                {
                    Encoding encoding = GetEncoding(wf);
                    ret = File.ReadAllLines(wf, encoding);
                }
            }
            catch
            {
                MessageBox.Show("Error opening file - " + wf);
                ret = null;
            }

            return ret;
        }
        /// <summary>
        /// Load digrams
        /// </summary>
        private void readDigrams()
        {
            string[] dgms = null;
            string f = digramPath + digramFile;
            try
            {
                if (File.Exists(f))
                {
                    Encoding encoding = GetEncoding(f);
                    dgms = File.ReadAllLines(f, encoding);
                    let2.Clear();
                    foreach (string s in dgms)
                    {
                        if(s.Length>0)
                        {
                            let2.Add(s);
                        }
                    }
                }

            }
            catch
            {
                MessageBox.Show("Error opening file - " + f);

            }

        }
        private void buildWordTree()
        {
            wordFileName = toolStripWordLists.Text;
            backgroundWorker1.RunWorkerAsync();
            pbar.setup("Loading word list", "", false, true);
            pbar.setProgress(0, "0 entries loaded");
            pbar.StartPosition = FormStartPosition.CenterParent;
            pbar.bkWorker = backgroundWorker1;
            pbar.ShowDialog();

        }
        /// <summary>
        /// builds the anagram and non-anagram trees from the word list.
        /// </summary>
        private void backgroundWorker1_buildWordTree(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(1000);
            StringBuilder sb = new StringBuilder("");

            //try
            {

                //this.Enabled = false;
                //this.UseWaitCursor = true;
                //Application.DoEvents();


                //read the word file
                words = readWords();
                wordTree = new treeElement();
                wordTreeSorted = new treeElement();
                if (words == null) //nothing to be seen here...move along!
                {
                    return;
                }
                numEntries = words.Length;
                countEntries = 0;
                int reportDiv = numEntries / 100;
                foreach (string word in words)
                {
                    countEntries++;
                    string tmpWord = RemoveAccentuation(word.Trim(' ').ToLower(), false);
                    List<char> letters = tmpWord.ToList<char>();

                    //build the non-anagram tree
                    Dictionary<char, treeElement> tdict = wordTree.dict;
                    int numlet = letters.Count;
                    int idx = 0;
                    foreach (char c in letters)
                    {
                        idx++;
                        if (c == '\'' || c == '-' || c == ' ' || c == '&')
                        {
                            continue; //skip these chars
                        }
                        if (!tdict.ContainsKey(c))
                        {
                            tdict[c] = new treeElement();
                        }
                        if (idx == numlet)
                        {
                            tdict[c].words.Add(word);
                        }
                        tdict = tdict[c].dict;
                    }
                    //build the anagram tree
                    letters.Sort();
                    tdict = wordTreeSorted.dict;
                    idx = 0;
                    sb.Clear();
                    foreach (char c in letters)
                    {
                        idx++;
                        if (c == '\'' || c == '-' || c == ' ' || c == '&')
                        {
                            continue; //skip these chars
                        }
                        sb.Append(c);
                        if (!tdict.ContainsKey(c))
                        {
                            tdict[c] = new treeElement();
                        }
                        if (idx == numlet)
                        {
                            tdict[c].words.Add(word);
                        }
                        tdict = tdict[c].dict;
                    }

                    if ((countEntries % reportDiv) == 0 && countEntries < numEntries)
                    {
                        backgroundWorker1.ReportProgress(100 * countEntries / numEntries);
                    }
                }
                backgroundWorker1.ReportProgress(100);
                Thread.Sleep(300);
            }
            //  finally
            //   {
            //     this.Enabled = true;
            //     this.UseWaitCursor = false;

            //   }

        }
        /// <summary>
        /// Recursive function to find matching words based on specified mutations and edit distance
        /// </summary>
        /// <param name="wordbit">Remaining input letters</param>
        /// <param name="distance">remaining edit distance</param>
        /// <param name="treeNode">current element in word tree</param>
        /// <param name="nomutate">don't do substitutions</param>
        /// <param name="nodelete">don't do delete</param>
        /// <param name="noinsert">don't do insert</param>
        /// <param name="ninsert">number of insertions available</param>
        /// <param name="ndelete">number of deletions available</param>
        /// <param name="lastch">character in parent node</param>
        /// <param name="deleted">flag to say parent character deleted</param>
        private void findMatch(string wordbit, int distance, treeElement treeNode, bool nomutate, bool nodelete, bool noinsert, int ninsert, int ndelete, char lastch, bool deleted)
        {
            string fstletter = "";
            char fstchar = (char)0;
            if (distance < 0) //edit distance exceeded so return
                return;
            if (distance < ndelete || distance < ninsert)
                return;
            bool fixedChar = false;
            if (backgroundWorker2.CancellationPending)
            {
                return;
            }

            if (wordbit.Length > 0)
            {
                fstletter = wordbit.Substring(0, 1).ToLower();
                fstchar = fstletter.First();
                if (fstletter != wordbit.Substring(0, 1))
                {
                    fixedChar = true;
                }
            }
            if (numberOfWords > 1)
            {

            }
            if (wordbit.Length > 0 && distance == 0 && ninsert == 0 && ndelete == 0) //no more mutations so flush to end of letters
            {
                if (treeNode.dict.ContainsKey(fstchar)) //letter found on this level so descend
                {
                    findMatch(wordbit.Substring(1), distance, treeNode.dict[fstchar], nomutate, nodelete, noinsert, ninsert, ndelete, (char)0, false);
                    return;
                }
            }

            //try insertion
            int nih = ninsert;
            int disth = distance;
            if (disth > 0 && (nih > 0 || !noinsert))
            {
                if (nih > 0)
                {
                    nih--;
                    if (ndelete > 0)
                    {
                        disth++;
                    }
                }
                foreach (char letter in treeNode.dict.Keys)
                {
                    if (!deleted || letter != lastch)
                    {
                        findMatch(wordbit, disth - 1, treeNode.dict[letter], nomutate, nodelete, noinsert, nih, ndelete, letter, false);
                    }
                }
            }
            //node is a word so add it if remaining length within edit distance
            if (treeNode.words.Count > 0)
            {
                if (wordbit.Length <= distance)
                {
                    if (ndelete == 0 && ninsert == 0 && (wordbit.Length == 0 || !nodelete) && distance == 0)
                    {
                        foreach (string w in treeNode.words)
                        {

                            if (match(RemoveAccentuation(w.ToLower(), false)) && !foundWords.Contains(w))
                            {
                                foundWords.Add(w);
                                if ((foundWords.Count % 10) == 0)
                                {
                                    backgroundWorker2.ReportProgress(1);
                                }
                            }
                        }
                    }
                }
            }

            if (wordbit.Length == 0) //all done
                return;
            //mutate
            foreach (char letter in treeNode.dict.Keys) //substitute each letter on level in turn
            {
                if (wordbit.Length > 0)
                {
                    if (letter != fstchar) // change letter and descend to next level
                    {
                        if (!nomutate && !fixedChar)
                        {
                            findMatch(wordbit.Substring(1), distance - 1, treeNode.dict[letter], nomutate, nodelete, noinsert, ninsert, ndelete, (char)0, false);
                        }
                    }
                    else //not a mutation, just descend
                    {
                        findMatch(wordbit.Substring(1), distance, treeNode.dict[fstchar], nomutate, nodelete, noinsert, ninsert, ndelete, (char)0, false);
                    }
                }
            }
            //try deletion
            disth = distance;
            if (!fixedChar)
            {
                if (ndelete > 0 || !nodelete)
                {
                    if (ndelete > 0)
                    {
                        ndelete--;
                        if (ninsert > 0)
                        {
                            disth++;
                        }
                    }
                    if (deleted || fstchar != lastch) //skip insignificant deletions 
                    {
                        findMatch(wordbit.Substring(1), disth - 1, treeNode, nomutate, nodelete, noinsert, ninsert, ndelete, fstchar, true);
                    }
                }
            }
            return;

        }
        /// <summary>
        /// Check string against current regex pattern
        /// </summary>
        /// <param name="wordbit">thing to check</param>
        /// <returns>true if matched</returns>
        private bool match(string wordbit)
        {
            if (filter.Length == 0 || re == null)
            {
                return true;
            }
            Match mat = re.Match(wordbit);
            return mat.Success;
        }
        /// <summary>
        /// refresh list of files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            cbWordListsRefresh(false);
        }
        /// <summary>
        /// do the work. Find matches using current settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //find words
            startDoSearch();

        }
        /// <summary>
        /// Kicks off the search
        /// </summary>
        private void startDoSearch()
        {
            gChkSubst = chkSubst.Checked;
            gChkDeletion = chkDeletion.Checked;
            gChkInsertion = chkInsertion.Checked;
            gChkAnagram = chkAnagram.Checked;
            gChkUseDiacritic = chkUseDiacritic.Checked;
            gChkDoAll = chkDoAll.Checked;
            gNumMutations = (int)numMutations.Value;
            gNumWords = (int)numWords.Value;
            gNumMinLength = (int)numMinLength.Value;

            foundWords.Clear(); //clear the found list
            multiWords.Clear();
            lbWords.Items.Clear(); // clear the output listbox
            filter = "";
            updatePattern(cbPattern.Text);
            filter = RemoveAccentuation(cbPattern.Text.ToLower(), true); //get the pattern
            re = null;
            try
            {
                if (filter != "")
                {
                    if (chkBeg.Checked && filter.IndexOf('^') != 0)
                    {
                        filter = "^" + filter;
                    }
                    if (chkEnd.Checked && filter.IndexOf('$') != filter.Length - 1)
                    {
                        filter = filter + "$";
                    }
                    re = new Regex(filter);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in match pattern. " + ex.Message);
                re = null;
            }
            gWordIn = RemoveAccentuation(cbLetters.Text.ToLower(), true);
            updateLetters(cbLetters.Text);

            backgroundWorker2.RunWorkerAsync();
            pbar.setup("Finding words", "", true, false);
            pbar.setProgress(0, "0 entries found");
            pbar.bkWorker = backgroundWorker2;
            pbar.ShowDialog(); //this will block until thread complete
            //we're done - add words to list
            lblEntries.Text = "Entries found: " + foundWords.Count;
            lblEntries.Refresh();
            this.SuspendLayout();
            foreach (string w in foundWords) //add words to output listbox
            {
                if ((chkAnagram.Checked && chkSubst.Checked && inLetters.Count>0))
                {
                    //normal anagram. Get rid of dodgy characters (nb, may need extending) before checking word length
                    string tw = w.Replace(" ", "").Replace("-", "").Replace("'", "");

                    if (tw.Length == inLetters.Count)
                    {
                        lbWords.Items.Add(w);
                    }
                }
                else
                {
                    lbWords.Items.Add(w);
                }
            }
            this.ResumeLayout();



        }
        private void doSearch(object sender, DoWorkEventArgs e)
        {
            try
            {
                //                this.Enabled = false;
                //                this.UseWaitCursor = true;
                //                Application.DoEvents();

                //create sorted list of letters in case anagram required.

                string wordIn = gWordIn;
                if (wordIn == "" && filter != "")
                {
                    foreach (string w in words)
                    {

                        if (match(RemoveAccentuation(w.ToLower(), false)))
                        {
                            foundWords.Add(w);
                        }
                    }
                }
                else
                {
                    //delete current input characters
                    foreach (List<char> c in inLetters)
                    {
                        c.Clear();
                    }
                    inLetters.Clear();
                    //create character input structure
                    List<char> letters = wordIn.ToList();
                    bool inLetterList = false;
                    List<char> tmp = null;

                    foreach (char c in letters)
                    {
                        //start a list of protected characters
                        if (!inLetterList && c == '[')
                        {
                            inLetterList = true;
                            tmp = new List<char>();
                        }
                        //close list of protected characters
                        else if (inLetterList && c == ']')
                        {
                            if (tmp.Count > 0)
                            {
                                inLetters.Add(tmp);
                            }
                            inLetterList = false;
                        }
                        else
                        {
                            //add characters to string or list
                            if (tmp == null || !inLetterList)
                            {
                                tmp = new List<char>();
                                inLetters.Add(tmp);
                            }
                            if (inLetterList)
                            {
                                tmp.Add(char.ToUpper(c));

                            }
                            else
                            {
                                tmp.Add(c);
                            }
                        }
                    }
                    //retrieve settings
                    int ipos = 0;
                    //get mutation types required
                    bool nomutate = !gChkSubst;
                    bool nodelete = !gChkDeletion;
                    bool noinsert = !gChkInsertion;
                    //get edit distance
                    int distance = gNumMutations;
                    numberOfWords = gNumWords;
                    int numWordsLeft = numberOfWords;
                    int ndelete = 0;
                    int ninsert = 0;
                    int startDistance = distance;
                    //if do all selected then iterate all mutations
                    if (numberOfWords > 1) //if multi then ignore distance
                    {
                        startDistance = 0;
                        distance = 0;
                    }
                    else if (gChkDoAll)
                    {
                        startDistance = 0;
                    }
                    while (distance >= startDistance)
                    {
                        generateInput(ipos, "", distance, nomutate, nodelete, noinsert, ninsert, ndelete);
                        if (backgroundWorker2.CancellationPending)
                        {
                            break;
                        }

                        distance--;
                    }
                }
            }
            finally
            {
                //    this.UseWaitCursor = false;
                //    this.Enabled = true;


            }
            if (backgroundWorker2.CancellationPending)
            {
                e.Cancel = true;
            }
            else
            {
                backgroundWorker2.ReportProgress(100);
            }
            Thread.Sleep(300);


        }
        /// <summary>
        /// recursive process iterating over constraints to generate all strings from input. Call findmatch once string is assembled.
        /// </summary>
        /// <param name="ipo"></param>
        /// <param name="s"></param>
        /// <param name="distance"></param>
        /// <param name="nomutate"></param>
        /// <param name="nodelete"></param>
        /// <param name="noinsert"></param>
        /// <param name="ninsert"></param>
        /// <param name="ndelete"></param>
        void generateInput(int ipo, string s, int distance, bool nomutate, bool nodelete, bool noinsert, int ninsert, int ndelete)
        {
            if (ipo == inLetters.Count)
            {
                List<char> letters = s.ToList();
                List<string> sletters = new List<string>();
                foreach (char c in letters)
                {
                    sletters.Add("" + c);
                }

                sletters.Sort(StringComparer.OrdinalIgnoreCase);
                string wordSorted = "";
                foreach (string c in sletters)
                {
                    wordSorted = wordSorted + c;
                }


                // set ndelete=ninsert=distance for anagram finding and switch off substitution. An insert and delete operation 
                // simulates substitution for anagrams
                if (gChkAnagram && gChkSubst)
                {
                    //ndelete = distance;
                    //ninsert = distance;
                    distance *= 2;
                    nomutate = true;
                    nodelete = false;
                    noinsert = false;
                }
                if (gChkAnagram || numberOfWords > 1)//use sorted word and tree for anagrams
                {
                    if (numberOfWords < 2)
                    {
                        findMatch(wordSorted, distance, wordTreeSorted, nomutate, nodelete, noinsert, ninsert, ndelete, (char)0, false);

                    }
                    else
                    {
                        currentLetters = wordSorted;
                        if (wordSorted.Length > 1 && wordTreeSorted.dict.ContainsKey(wordSorted[0]))
                        {
                            findWords(wordSorted.Substring(1), wordTreeSorted.dict[wordSorted[0]], "", 0);

                        }

                    }


                }
                else //normal find
                {
                    findMatch(s, distance, wordTree, nomutate, nodelete, noinsert, ninsert, ndelete, (char)0, false);
                }
            }
            else
            {
                List<char> chars = inLetters[ipo];
                ipo++;
                foreach (char c in chars)
                {
                    string sc = "" + c;
                    generateInput(ipo, s + sc, distance, nomutate, nodelete, noinsert, ninsert, ndelete);
                    if (backgroundWorker2.CancellationPending)
                    {
                        return;
                    }

                }
            }

        }
        private void findWords(string letters, treeElement treeNode, string phrase, int wcount)
        {
            if (wcount > numberOfWords)
                return;
            char fstchar;
            if (backgroundWorker2.CancellationPending)
            {
                return;
            }

                if (treeNode.words.Count > 0)
            {
                foreach (string w in treeNode.words)
                {
                    string tmpPhrase = phrase;

                    if (w.Length >= gNumMinLength && (w.Length > 2 || let2.Contains(RemoveAccentuation(w.ToLower(), false))))
                    {
                        tmpPhrase = phrase + (phrase.Length == 0 ? "" : " ") + w;
                        for (int i = 0; i < letters.Length; i++)
                        {
                            fstchar = letters[i];
                            string remLetters = letters.Substring(i + 1);
                            if (wordTreeSorted.dict.ContainsKey(fstchar)) //letter found on this level so descend
                            {
                                findWords(remLetters, wordTreeSorted.dict[fstchar], tmpPhrase, wcount + 1);
                            }
                        }

                        if (wcount + 1 == numberOfWords)
                        {
                            string tmpletters = currentLetters;
                            string tmp = RemoveAccentuation(tmpPhrase.ToLower(), false);
                            for (int j = 0; j < tmp.Length; j++)
                            {
                                int ndx = tmpletters.IndexOf(tmp[j]);
                                if (ndx > -1)
                                {
                                    tmpletters = tmpletters.Remove(ndx, 1);
                                }
                            }

                            if (tmpletters.Length == 0)
                            {
                                foundWords.Add(tmpPhrase);
                                if ((foundWords.Count % 10) == 0)
                                {
                                    backgroundWorker2.ReportProgress(1);
                                }

                            }
                        }
                    }
                }

            }


            for (int i = 0; i < letters.Length; i++)
            {
                fstchar = letters[i];
                string remLetters = "";
                if (letters.Length > 1)
                {
                    remLetters = letters.Remove(i, 1);
                }
                if (treeNode.dict.ContainsKey(fstchar)) //letter found on this level so descend
                {
                    findWords(remLetters, treeNode.dict[fstchar], phrase, wcount);

                }
            }


            return;
        }
        /// <summary>
        /// lbWords event handler. Double clicking will attempt to look up word using current url from lbUrls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbWords_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            browseWord(); //use current url to look up selected word
        }
        /// <summary>
        /// invoke browser with current url and word
        /// </summary>
        private void browseWord()
        {
            if (lbWords.SelectedIndex > -1 && lbUrl.SelectedIndex > -1)
            {
                string ur = lbUrl.Items[lbUrl.SelectedIndex].ToString();
                string w = lbWords.Items[lbWords.SelectedIndex].ToString();
                ur = ur.ToLower().Replace("%word%", w); //replace placeholder with word
                updateHistory(ur, true);
            }

        }
        /// <summary>
        /// Maintain the input string combobox. Remove current item and insert at head of cb
        /// </summary>
        /// <param name="txt"></param>
        void updateLetters(string txt)
        {
            int idx = cbLetters.Items.IndexOf(txt);
            if (idx > -1)
            {
                cbLetters.Items.RemoveAt(idx);
            }
            cbLetters.Items.Insert(0, txt);
            applyCBLimit(cbLetters, maxHistory);
            cbLetters.SelectedIndex = 0;
        }
        /// <summary>
        /// Limit the number of items in a combo box (used for histories)
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="maxItems"></param>
        void applyCBLimit(ComboBox cb, int maxItems)
        {
            if (maxItems < 1)
            {
                return;
            }
            if (cb.Items.Count > maxItems)
            {
                for (int i = cb.Items.Count - 1; i > maxItems; i--)
                {
                    cb.Items.RemoveAt(i);
                }
            }

        }
        /// <summary>
        /// Limit the number of items in a combo box (used for histories)
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="maxItems"></param>
        void applyToolStripCBLimit(ToolStripComboBox cb, int maxItems)
        {
            if (maxItems < 1)
            {
                return;
            }
            if (cb.Items.Count > maxItems)
            {
                for (int i = cb.Items.Count - 1; i > maxItems; i--)
                {
                    cb.Items.RemoveAt(i);
                }
            }

        }
        /// <summary>
        ///  Maintain the pattern string combobox. Remove current item and insert at head of cb
        /// </summary>
        /// <param name="txt"></param>
        void updatePattern(string txt)
        {
            int idx = cbPattern.Items.IndexOf(txt);
            if (idx > -1)
            {
                cbPattern.Items.RemoveAt(idx);
            }
            cbPattern.Items.Insert(0, txt);
            applyCBLimit(cbPattern, maxHistory);
            cbPattern.SelectedIndex = 0;
        }
        /// <summary>
        ///  Maintain the input url history combobox. Remove current item and insert at head of cb
        /// </summary>
        /// <param name="ur"></param>
        /// <param name="doNavigate"></param>
        void updateHistory(string ur, bool doNavigate)
        {
            int idx = toolStripHistory.Items.IndexOf(ur);
            if (idx > -1)
            {
                toolStripHistory.Items.RemoveAt(idx);
            }
            toolStripHistory.Items.Insert(0, ur);
            applyToolStripCBLimit(toolStripHistory, maxHistory);

            toolStripHistory.SelectedIndex = 0;
            if (doNavigate)
            {
                webBrowser1.ScriptErrorsSuppressed = true; //suppress script warnings
                webBrowser1.Navigate(ur); // go get page
            }

        }
        /// <summary>
        /// Changing url causes web look up to refresh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbUrl_SelectedIndexChanged(object sender, EventArgs e)
        {
            browseWord(); //use new url to look up word
        }
        /// <summary>
        /// lbWords context menu handler to copy current selection to clipboard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbWords.SelectedIndex > -1) //copy current selected item to clipboard
            {
                string w = lbWords.Items[lbWords.SelectedIndex].ToString();
                Clipboard.SetText(w);
            }
            else
            {
                Clipboard.SetText("");
            }
        }
        /// <summary>
        /// Change lbWords selection on any mouse down event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbWords_MouseDown(object sender, MouseEventArgs e)
        {
            lbWords.SelectedIndex = lbWords.IndexFromPoint(e.X, e.Y); //set current selected item on mouse down
        }
        /// <summary>
        /// Copy all words in lbWords to clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyAllToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //copy all words in lbWords to clipboard.
            StringBuilder sb = new StringBuilder("");
            foreach (string w in lbWords.Items)
            {
                sb.AppendLine(w);
            }

            Clipboard.SetText(sb.ToString());
        }
        /// <summary>
        ///Removes accents etc - based on method suggested by Sergio Cabral in Stack Overflow
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string RemoveAccentuation(string text, bool isPattern)
        {
            string ret = "";
            if (gChkUseDiacritic)
            {
                return text;
            }
            //preserve ?s in a pattern
            if (isPattern)
            {
                text = text.Replace("?", "q_q");
            }
            //preserve german ß character
            int idx = text.IndexOf("ß");
            if (idx > -1)
            {
                text = text.Replace("ß", "z_q");
                ret = System.Web.HttpUtility.UrlDecode(
                    System.Web.HttpUtility.UrlEncode(
                        text, Encoding.GetEncoding("iso-8859-7")));

                //reinstate ß character
                ret = ret.Replace("z_q", "ß");

            }
            else
            {
                ret = System.Web.HttpUtility.UrlDecode(
                    System.Web.HttpUtility.UrlEncode(
                        text, Encoding.GetEncoding("iso-8859-7")));

            }
            //escape any ?s geenrated by encoding
            ret = ret.Replace("?", "\\?");
            //reinstate any ? in patterns
            if (isPattern)
            {
                ret = ret.Replace("q_q", "?");
            }
            return ret;


        }
        private void cbWordListsRefresh(bool forceRead)
        {
            if (Directory.Exists(listPath))
            {
                string[] files = Directory.GetFiles(listPath);
                int idx = 0;
                toolStripWordLists.Items.Clear();
                foreach (string f in files)
                {
                    if (File.Exists(f))
                    {
                        string fna = f.ToLower().Replace(listPath.ToLower(), "");
                        toolStripWordLists.Items.Add(fna);
                        if (fna == wordFile.ToLower())
                        {
                            idx = toolStripWordLists.Items.Count - 1;
                        }
                    }
                }
                if (toolStripWordLists.Items.Count > 0)
                {
                    toolStripWordLists.SelectedIndex = idx;
                }
                else
                {
                    wordFile = "";
                }
                
                files = Directory.GetFiles(digramPath);
                idx = 0;
                toolStripDigram.Items.Clear();
                foreach (string f in files)
                {
                    string fna = f.ToLower().Replace(digramPath.ToLower(), "");
                    toolStripDigram.Items.Add(fna);
                    if (fna == digramFile.ToLower())
                    {
                        idx = toolStripDigram.Items.Count - 1;
                    }
                }
                if (toolStripDigram.Items.Count > 0)
                {
                    toolStripDigram.SelectedIndex = idx;
                }
                else
                {
                    digramFile = "";
                }
            }
        }
        /// <summary>
        /// Load new word list on change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripWordLists_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (toolStripWordLists.Items[toolStripWordLists.SelectedIndex].ToString() != wordFile.ToLower())
            {
                wordFile = toolStripWordLists.Items[toolStripWordLists.SelectedIndex].ToString();
                buildWordTree();

            }
        }

        /// <summary>
        /// trap post navigate event. Doesn't work too well as gets adverts and the like. Disabled for now.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            //string ur = e.Url.ToString();

            //updateHistory(ur, false);
        }
        /// <summary>
        /// change of diacritic box causes reload
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkUseDiacritic_CheckedChanged(object sender, EventArgs e)
        {
            gChkUseDiacritic = chkUseDiacritic.Checked;

            buildWordTree();
        }
        /// <summary>
        /// not used currently
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WordList_Load(object sender, EventArgs e)
        {

        }
        private void restoreSession()
        {
            if (File.Exists(dataPath + "lastsession.txt"))
            {
                string[] a = File.ReadAllLines(dataPath + "lastsession.txt");
                foreach (string s in a)
                {
                    string strimmed = s.Trim(' ');
                    int idx = strimmed.IndexOf('=');
                    if (idx > 0 && idx < strimmed.Length - 1)
                    {
                        string sleft = strimmed.Substring(0, idx).ToLower();
                        string sright = strimmed.Substring(idx + 1);
                        switch (sleft)
                        {

                            case "wordlist":
                                {
                                    wordFileName = sright;
                                    break;
                                }
                            case "digram":
                                {
                                    digramFileName = sright;
                                    break;
                                }
                            case "nummutations":
                                {
                                    int n = 0;
                                    if (int.TryParse(sright, out n))
                                    {
                                        numMutations.Value = n;
                                    }
                                    break;
                                }
                            case "chkanagram":
                                {
                                    chkAnagram.Checked = (sright == "y");
                                    break;
                                }
                            case "chkdeletion":
                                {
                                    chkDeletion.Checked = (sright == "y");
                                    break;
                                }
                            case "chkdoall":
                                {
                                    chkDoAll.Checked = (sright == "y");
                                    break;
                                }
                            case "chksubst":
                                {
                                    chkSubst.Checked = (sright == "y");
                                    break;
                                }
                            case "chkinsertion":
                                {
                                    chkInsertion.Checked = (sright == "y");
                                    break;
                                }
                            case "lasturl":
                                {
                                    lastURL = sright;
                                    break;
                                }
                            case "pattern":
                                {
                                    cbPattern.Items.Add(sright);
                                    break;
                                }
                            case "letters":
                                {
                                    cbLetters.Items.Add(sright);
                                    break;
                                }
                            case "urlhistory":
                                {
                                    toolStripHistory.Items.Add(sright);
                                    break;
                                }
                            default:
                                break;
                        }
                    }

                }
                updateLetters("");
                updatePattern("");
                updateHistory("", false);
            }
        }
        /// <summary>
        /// Write out history and current settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WordList_FormClosing(object sender, FormClosingEventArgs e)
        {
            StringBuilder sb = new StringBuilder("");
            //chkbox states
            sb.AppendLine("chkAnagram=" + (chkAnagram.Checked ? "y" : "n"));
            sb.AppendLine("chkDeletion=" + (chkDeletion.Checked ? "y" : "n"));
            sb.AppendLine("chkDoAll=" + (chkDoAll.Checked ? "y" : "n"));
            sb.AppendLine("chkSubst=" + (chkSubst.Checked ? "y" : "n"));
            sb.AppendLine("chkInsertion=" + (chkInsertion.Checked ? "y" : "n"));
            sb.AppendLine("chkUseDiacritic=" + (chkUseDiacritic.Checked ? "y" : "n"));
            sb.AppendLine("numMutations=" + numMutations.Value.ToString());
            sb.AppendLine("wordList=" + wordFile);
            sb.AppendLine("digramFile=" + digramFile);
            int idx = lbUrl.SelectedIndex;
            if (idx > -1)
            {
                sb.AppendLine("lasturl=" + lbUrl.Items[lbUrl.SelectedIndex].ToString());
            }
            foreach (string s in cbPattern.Items)
            {
                sb.AppendLine("pattern=" + s);
            }
            foreach (string s in cbLetters.Items)
            {
                sb.AppendLine("letters=" + s);
            }
            foreach (string s in toolStripHistory.Items)
            {
                sb.AppendLine("urlHistory=" + s);
            }
            File.WriteAllText(dataPath + "lastsession.txt", sb.ToString());

        }
        /// <summary>
        /// disable/enable controls as necessary
        /// </summary>
        private void checkEnable()
        {
            bool hasLetters = false;
            bool hasPattern = false;
            bool isNotMulti = true;

            if (cbLetters.Text != null && cbLetters.Text != "")
            {
                hasLetters = true;
            }
            if (cbPattern.Text != null && cbPattern.Text != "")
            {
                hasPattern = true;
            }
            if (numWords.Value > 1)
            {
                isNotMulti = false;
            }
            //go button
            button2.Enabled = hasLetters || hasPattern;
            chkAnagram.Enabled = hasLetters && isNotMulti;
            chkDeletion.Enabled = hasLetters && isNotMulti;
            chkSubst.Enabled = hasLetters && isNotMulti;
            chkInsertion.Enabled = hasLetters && isNotMulti;
            chkDoAll.Enabled = hasLetters && isNotMulti;
            numMutations.Enabled = hasLetters && isNotMulti;
            numMinLength.Enabled = (hasLetters || !hasPattern) && !isNotMulti;
            cbPattern.Enabled = isNotMulti;
            numWords.Enabled = hasLetters || !hasPattern;


        }

        /// <summary>
        /// Run search if enter is pressed in the cbLetters combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbLetters_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                startDoSearch();
            }

        }
        /// <summary>
        /// Rub=n search if enter pressed in cbpattern combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbPattern_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                startDoSearch();
            }

        }
        /// <summary>
        /// remove letters from source input 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeLettersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gChkUseDiacritic = chkUseDiacritic.Checked;
            string wordIn = RemoveAccentuation(cbLetters.Text.ToLower(), true);
            if (lbWords.SelectedIndex > -1) //copy current selected item to clipboard
            {
                string w = RemoveAccentuation(lbWords.Items[lbWords.SelectedIndex].ToString().ToLower(), false);
                char[] ch = w.ToCharArray();
                foreach (char c in ch)
                {
                    int idx = wordIn.IndexOf(c);
                    if (idx > -1)
                    {
                        wordIn = wordIn.Remove(idx, 1);
                    }
                }
                cbLetters.Text = wordIn;

            }


        }
        /// <summary>
        /// enable/disable controls on change of source letters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbLetters_TextChanged(object sender, EventArgs e)
        {
            checkEnable();
        }

        private void cbPattern_TextChanged(object sender, EventArgs e)
        {
            checkEnable();
        }
        /// <summary>
        /// Enable or disable controls on change of pattern
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numWords_ValueChanged(object sender, EventArgs e)
        {
            checkEnable();
        }
        /// <summary>
        /// initialise app when form is shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WordList_Shown(object sender, EventArgs e)
        {
            buildWordTree();

            // instantiate output word set
            foundWords = new SortedSet<string>();
            multiWords = new SortedSet<string>();
            checkEnable();

        }
        /// <summary>
        /// open selected wordlist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripWordLists_SelectedChangeCommitted(object sender, EventArgs e)
        {
            if (toolStripWordLists.Items[toolStripWordLists.SelectedIndex].ToString() != wordFile.ToLower())
            {
                wordFile = toolStripWordLists.Items[toolStripWordLists.SelectedIndex].ToString();
                buildWordTree();

            }
        }
        /// <summary>
        /// open url when enter pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripHistory_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string ur = toolStripHistory.Text;
                e.Handled = true;
                updateHistory(ur, true);
            }

        }
        /// <summary>
        /// use selected url
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripHistory_SelectedChangeCommitted(object sender, EventArgs e)
        {
            if (toolStripHistory.SelectedIndex > -1)
            {
                string ur = toolStripHistory.Items[toolStripHistory.SelectedIndex].ToString();
                updateHistory(ur, true);
            }

        }
        /// <summary>
        /// Load new digram list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripDigram_SelectedChangeCommitted(object sender, EventArgs e)
        {
            if (toolStripDigram.Items[toolStripDigram.SelectedIndex].ToString() != digramFile.ToLower())
            {
                digramFile = toolStripDigram.Items[toolStripDigram.SelectedIndex].ToString();
                readDigrams();

            }

        }
    }
    /// <summary>
    /// Class treeElement contains a dictionary of letters with associated treeElements. Input word lists are inserted into this structure
    /// words list of words relevent to an element. For lists sorted for anagrams there may be more than one word (all anags of each other)
    /// For non-anagram searches there will only be one word in any list.
    /// </summary>
    class treeElement
    {
        public Dictionary<char, treeElement> dict;
        public SortedSet<string> words;
        public treeElement()
        {
            dict = new Dictionary<char, treeElement>();
            words = new SortedSet<string>();
        }
    }
}