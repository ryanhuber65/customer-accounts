using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
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



namespace COMP255FinalProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //List array that stores CustomerAccount objects
        private List<CustomerAccount> CustomerList = new List<CustomerAccount>();
        private CustomerAccount CurrentCustomer;
        //List array that stores AccountTransaction objects
        private List<AccountTransaction> TransactionList = new List<AccountTransaction>();
        private AccountTransaction CurrentTransaction;
        //CurrentRecord that stores an index of a selected item in a listbox
        private int CurrentRecord;
        //set the Transaction Number at the next corresponding value
        private int TransactionValue = 1502;
        //set the Account Number at the next corresponding value
        private int AccountNum = 6;
        public MainWindow()
        {
            InitializeComponent();
            //Set the DataDirectory Substitution String
            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(executable));
            AppDomain.CurrentDomain.SetData("DataDirectory", path);
            //load the existing data
            LoadCustomer();
            LoadTransaction();
        }
        //Method to Add a customer 
        private void AddCustomer(int Account, string FirstName, string LastName, string Email, string Phone)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                //new class object
                CustomerAccount CurrentCustomer1 = new CustomerAccount(Account, FirstName, LastName, Email, Phone);
                //point connection to database
                connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\CustomerAccounts.mdf;Integrated Security=True";
                //open the connection
                connection.Open();
                // Format and execute SQL statement.
                // setup a SQL Command  - here Insert
                string sql = "Insert Into CustomerAccounts" +
                             "(AccountNumber, FirstName, LastName, Email, Phone) Values" +
                             $"('{CurrentCustomer1.AccountNumber}', " +
                             $"'{CurrentCustomer1.FirstName}', " +
                             $"'{CurrentCustomer1.LastName}', " +
                             $"'{CurrentCustomer1.Email}', " +
                             $"'{CurrentCustomer1.Phone}')";
                // set the SQL Command string on the connection
                // in a new connection object
                // run the Insert command - use command.ExecuteNonQuery()
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
                //call the LoadCustomer methodLoadTransaction();
                CustomerList.Add(CurrentCustomer1);

                // add the contact to the listbox
                CustomersListBox.Items.Add(CurrentCustomer1);

                //call the DisplayRecord method
                DisplayCustomerRecord(CurrentCustomer1);
            }
        }
        //Method to add a trransaction
        private void AddTransaction(int TransactionNumber, int AccountNumber, DateTime TransactionDate, decimal TransactionAmount)
        {
            //declare a connection object
            using (SqlConnection connection = new SqlConnection())
            {
                //new class object
                AccountTransaction NewTransaction = new AccountTransaction(TransactionNumber, AccountNumber, TransactionDate, TransactionAmount);
                //point connection to database
                connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\CustomerAccounts.mdf;Integrated Security=True";
                //open the connection
                connection.Open();
                // Format and execute SQL statement.
                // setup a SQL Command  - here insert
                string sql = "Insert Into AccountTransactions" +
                             "(TransactionNumber, AccountNumber, TransactionDate, TransactionAmount) Values " +
                             $"('{NewTransaction.TransactionNumber}', " +
                             $"'{NewTransaction.AccountNumber}', " +
                             $"'{NewTransaction.TransactionDate}', " +
                             $"'{NewTransaction.TransactionAmount}' )";
                // set the SQL Command string on the connection
                // in a new connection object
                // run the Insert command - use command.ExecuteNonQuery()
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
                TransactionList.Add(NewTransaction);

                // add the contact to the listbox
                TransactionsListBox.Items.Add(NewTransaction);
                //call the LoadCustomer method
                LoadTransaction();
                //call the DisplayRecord method
                DisplayTransactionRecord();
            }
        }
        //Customer Data Validation method
        public bool IsCustomerDataValid()
        {

            //some extra validation for Customer data, error message displays if all fields aren't filled corectly
            if (txbFirstName.Text == "")
            {
                //error label will display this message if false
                errorMessageLabelAccount.Content = "Please enter the First Name correctly";
                //error label color is red
                errorMessageLabelAccount.Foreground = Brushes.Red;
                //focus on the textbox that needs to be corrected (First Name)
                txbFirstName.Focus();
                return false;
            }
            if (txbLastName.Text == "")
            {
                //error label will display this message if false
                errorMessageLabelAccount.Content = "Please enter the Last Name correctly";
                //error label color is red
                errorMessageLabelAccount.Foreground = Brushes.Red;
                //focus on the textbox that needs to be corrected (Last Name)
                txbLastName.Focus();
                return false;
            }
            if (txbEmail.Text == "")
            {
                //error label will display this message if false
                errorMessageLabelAccount.Content = "Please enter the Email correctly";
                //error label color is red
                errorMessageLabelAccount.Foreground = Brushes.Red;
                //focus on the textbox that needs to be corrected (Email)
                txbEmail.Focus();
                return false;
            }
            if (txbPhone.Text == "")
            {
                //if the phone number is left blank - assign it a N/A value
                txbPhone.Text = "N/A";
                //return true
                return true;
            }

            //if all fields are filled correcly returns as true

            return true;
        }
        //Transaction Data Validation Method
        public bool IsTransactionDataValid()
        {
            //unused variables that are strictly used forr validation purposes 
            decimal validation;
            DateTime validation2;

            //some extra validation for Customer data, error message displays if all fields aren't filled corectly
            //if textbox is blank or the entered value isn't a decimal, then false
            if (txbTransactionAmount.Text == "" || decimal.TryParse(txbTransactionAmount.Text, out validation) == false)
            {
                //error label will display this message if false
                errorMessageLabelTransaction.Content = "Please enter a correct Amount";
                //error label color is red
                errorMessageLabelTransaction.Foreground = Brushes.Red;
                //focus on the textbox that needs to be corrected (Transaction Amount)
                txbTransactionAmount.Focus();
                return false;
            }
            //if textbox is blank or the entered value isn't a DateTime, then false
            if (txbTransactionDate.Text == "" || DateTime.TryParse(txbTransactionDate.Text, out validation2) == false)
            {
                //error label will display this message if false
                errorMessageLabelTransaction.Content = "Please enter a correct Transaction Date";
                //error label color is red
                errorMessageLabelTransaction.Foreground = Brushes.Red;
                //focus on the textbox that needs to be corrected (Transaction Date)
                txbTransactionDate.Focus();
                return false;
            }
            //if all fields are filled correcly returns as true
            return true;
        }
        private void buttonNewTransaction_Click(object sender, RoutedEventArgs e)
        {
            //check if all data is valid
            if (IsTransactionDataValid() == false) return;

            //if data is valid procceed
            //variables used only in this click event
            int currentAccountNum = CustomersListBox.SelectedIndex + 1; ;
            //increment the Transaction Number for the next transaction
            TransactionValue++;
            //call the AddTransaction method to store all the data
            AddTransaction(TransactionValue,
                           currentAccountNum,
                           Convert.ToDateTime(txbTransactionDate.Text),
                           Convert.ToDecimal(txbTransactionAmount.Text));
        }
        private void buttonNew_Click(object sender, RoutedEventArgs e)
        {
            //check if the data is valid
            if (IsCustomerDataValid() == false) return;
            //if it is valid, proceed
            //blank the error message
            errorMessageLabelAccount.Content = "";
            //increment the Account Number for the next account
            AccountNum++;
            //call the AddCustomer method to store all the new data
            AddCustomer(AccountNum, txbFirstName.Text, txbLastName.Text, txbEmail.Text, txbPhone.Text);
        }
        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            //check validation
            if (IsCustomerDataValid() == false) return;
            //if data is valid, proceed
            //blank error message
            errorMessageLabelTransaction.Content = "";
            //call the save method
            SaveCustomer();
        }
        private void buttonSaveTransaction_Click(object sender, RoutedEventArgs e)
        {
            //check validation
            if (IsTransactionDataValid() == false) return;
            //if data is valid proceed
            //call the SaveTransaction method
            SaveTransaction();
        }
        //Save Customer Method
        public void SaveCustomer()
        {
            //blank the error message
            errorMessageLabelAccount.Content = "";
            //current record is the selected record(#) in the listbox
            CurrentRecord = CustomersListBox.SelectedIndex;
            //current customer is the selected object of the current item selected
            CurrentCustomer = (CustomerAccount)CustomersListBox.SelectedItem;
            //if the current record is -1 (nothing selected), proceed to an error message
            if (CurrentRecord == -1)
            {
                //if no record is selected, display this error message
                errorMessageLabelAccount.Content = "You must select an existing customer to save.";
                errorMessageLabelAccount.Foreground = Brushes.Red;
                return;
            }
            //if current record is not -1, proceed
            //assign all the updated textbox values into the properties of the Current object selected
            CurrentCustomer.FirstName = txbFirstName.Text;
            CurrentCustomer.LastName = txbLastName.Text;
            CurrentCustomer.Email = txbEmail.Text;
            CurrentCustomer.Phone = txbPhone.Text;
            //declare a connection object
            using (SqlConnection connection = new SqlConnection())
            {
                //point connection to database
                connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\CustomerAccounts.mdf;Integrated Security=True";
                //open the connection
                connection.Open();

                // Create a SQL command object.
                // setup a SQL Command  - here Update
                string sql = $"Update CustomerAccounts Set " +
                             $"FirstName = '{CurrentCustomer.FirstName}', " +
                             $"LastName = '{CurrentCustomer.LastName}', " +
                             $"Email = '{CurrentCustomer.Email}', " +
                             $"Phone = '{CurrentCustomer.Phone}' " +
                             $"Where AccountNumber = '{CurrentCustomer.AccountNumber}';";
                // set the SQL Command string on the connection
                // in a new connection object
                SqlCommand myCommand = new SqlCommand(sql, connection);
                // run the UPDATE command - use command.ExecuteNonQuery()
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
                //call this method
                LoadCustomer();
            }
        }
        //Save Transaction Method
        public void SaveTransaction()
        {
            //blank the error message
            errorMessageLabelTransaction.Content = "";
            //current record is the selected record(#) in the listbox
            CurrentRecord = TransactionsListBox.SelectedIndex;
            //current transaction is the current selected item in the transaction listbox
            CurrentTransaction = (AccountTransaction)TransactionsListBox.SelectedItem;
            //check validation if a record is selected
            if (CurrentRecord == -1)
            {
                //if no record is selected, display this error message
                errorMessageLabelTransaction.Content = "You must select an existing transaction to save.";
                errorMessageLabelTransaction.Foreground = Brushes.Red;
                return;
            }
            //if validation is correct, proceed
            //remove current transaction from the listbox
            TransactionList.Remove(CurrentTransaction);
            //update the new data
            CurrentTransaction.TransactionDate = Convert.ToDateTime(txbTransactionDate.Text);
            CurrentTransaction.TransactionAmount = Convert.ToDecimal(txbTransactionAmount.Text);
            //declare a connection object
            using (SqlConnection connection = new SqlConnection())
            {
                //point connection to database
                connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\CustomerAccounts.mdf;Integrated Security=True";
                //open the connection
                connection.Open();
                // Create a SQL command object.
                // setup a SQL Command  - here Update
                string sql = $"Update AccountTransactions Set " +
                             $"TransactionDate = '{CurrentTransaction.TransactionDate}', " +
                             $"TransactionAmount = '{CurrentTransaction.TransactionAmount}' " +
                             $"Where TransactionNumber = '{CurrentTransaction.TransactionNumber}';";
                // set the SQL Command string on the connection
                // in a new connection object
                SqlCommand myCommand = new SqlCommand(sql, connection);
                // run the UPDATE command - use command.ExecuteNonQuery()
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
                //call the load transaction method to update the new data and repopulate the list array
                LoadTransaction();
                //display the updated data
                DisplayTransactionRecord();
            }
        }
        //Load Existing Customer Data Method
        public void LoadCustomer()
        {
            //clear listbox and List array
            CustomerList.Clear();
            CustomersListBox.Items.Clear();
 
            //declare a connection object
            using (SqlConnection connection = new SqlConnection())
            {
                //point connection to database
                connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\CustomerAccounts.mdf;Integrated Security=True";
                //open the connection
                connection.Open();

                // Create a SQL command object.
                // setup a SQL Command  - here SELECT
                string sql = "SELECT* FROM CustomerAccounts Order By AccountNumber; ";
                // set the SQL Command string on the connection
                // in a new connection object
                SqlCommand myCommand = new SqlCommand(sql, connection);

                // Run the command: into a data reader using the ExecuteReader() method.
                using (SqlDataReader myDataReader = myCommand.ExecuteReader())
                {
                    // Loop over the results. myDataReader.Read returns false if we run out of rows
                    while (myDataReader.Read())
                    {
                        // each colum of the datareader goes into one property of a CustomerAccount Object
                        CustomerAccount NewCustomer = new CustomerAccount(myDataReader.GetInt32(0),
                                                                          myDataReader.GetString(1),
                                                                          myDataReader.GetString(2),
                                                                          myDataReader.GetString(3),
                                                                          myDataReader.GetString(4));
                        //add the Customer to the List<Contact>
                        CustomerList.Add(NewCustomer);

                        // add the contact to the listbox
                        CustomersListBox.Items.Add(NewCustomer);

                    }
                }
            }
        }
        //Load Existing Transaction Data Method
        public void LoadTransaction()
        {
            //clear listbox and List array
            TransactionList.Clear();
            TransactionsListBox.Items.Clear();

            //declare a connection object
            using (SqlConnection connection = new SqlConnection())
            {
                //point connection to database
                connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\CustomerAccounts.mdf;Integrated Security=True";
                //open the connection
                connection.Open();
                // Create a SQL command object.
                // setup a SQL Command  - here SELECT
                string sql = "SELECT* FROM AccountTransactions Order By AccountNumber; ";
                // set the SQL Command string on the connection
                // in a new connection object
                SqlCommand myCommand1 = new SqlCommand(sql, connection);

                // Run the command: into a data reader using the ExecuteReader() method.
                using (SqlDataReader myDataReader1 = myCommand1.ExecuteReader())
                {
                    // Loop over the results. myDataReader.Read returns false if we run out of rows
                    while (myDataReader1.Read())
                    {
                        // each colum of the datareader goes into one property of a AccountTransaction Object
                        AccountTransaction LoadTransaction = new AccountTransaction(myDataReader1.GetInt32(0),
                                                                                   myDataReader1.GetInt32(1),
                                                                                   myDataReader1.GetDateTime(2),
                                                                                   myDataReader1.GetDecimal(3));
                        //add the Customer to the List<Contact>
                        TransactionList.Add(LoadTransaction);
                    }
                }
            }
        }
        private void buttonDeletTransaction_Click(object sender, RoutedEventArgs e)
        {
            //blank the error message
            errorMessageLabelTransaction.Content = "";
            //variable exclusive to this click event
            int IndexToDelete;
            //If no contact is selected in the ListBox (index = -1) then show a warning
            if (TransactionsListBox.SelectedIndex == -1)
            {
                //if no record is selected to delete, display this error message
                errorMessageLabelTransaction.Content = "You must select a transaction to delete.";
                errorMessageLabelTransaction.Foreground = Brushes.Red;
                return;
            }
            //extra validation
            var Result = MessageBox.Show("Are you sure you want to delete " + CurrentTransaction.ToString() + "?",
                "ArrayList", MessageBoxButton.YesNo, MessageBoxImage.Question);
            //if yes is selected, delete the record from the listbox and list array
            if (Result == MessageBoxResult.Yes)
            {
                // record the index we are deleting
                IndexToDelete = CustomersListBox.SelectedIndex;
                //declare a connection object
                using (SqlConnection connection = new SqlConnection())
                {
                    // point connection to database
                    connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\CustomerAccounts.mdf;Integrated Security=True";
                    //open the connection
                    connection.Open();

                    // Create a SQL command object.
                    // setup a SQL Command  - here Delete
                    string sql = $"Delete from AccountTransactions where TransactionNumber = {CurrentTransaction.TransactionNumber}";
                    // set the SQL Command string on the connection
                    // in a new connection object
                    SqlCommand myCommand = new SqlCommand(sql, connection);

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // run the Delete command - use command.ExecuteNonQuery()
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            Exception error = new Exception("No record matching that TransactionNumber", ex);
                            throw error;
                        }
                        //Remove the contact from the ArrayList
                        TransactionList.Remove(CurrentTransaction);

                        //Remove the customer from the ListBox
                        TransactionsListBox.Items.Remove(CurrentTransaction);
                        getBalance();

                    }
                }
            }
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            int IndexToDelete;
            errorMessageLabelAccount.Content = "";
            //If no contact is selected in the ListBox (index = -1) then show a warning
            if (CustomersListBox.SelectedIndex == -1)
            {
                //if no record is selected to delete, display this error message
                errorMessageLabelAccount.Content = "You must select a customer to delete";
                errorMessageLabelAccount.Foreground = Brushes.Red;
                return;
            }
            //extra validation
            var Result = MessageBox.Show("Are you sure you want to delete " + CurrentCustomer.ToString() + "?",
                "ArrayList", MessageBoxButton.YesNo, MessageBoxImage.Question);
            //if yes is selected, delete the record from the listbox and list array
            if (Result == MessageBoxResult.Yes)
            {
                // record the index we are deleting
                IndexToDelete = CustomersListBox.SelectedIndex;
                //declare a connection object
                using (SqlConnection connection = new SqlConnection())
                {
                    // point connection to database
                    connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\CustomerAccounts.mdf;Integrated Security=True";
                    //open the connection
                    connection.Open();

                    // Create a SQL command object.
                    // setup a SQL Command  - here Delete
                    string sql = $"Delete from CustomerAccounts where AccountNumber = {CurrentCustomer.AccountNumber} " +
                                 $"Delete from AccountTransactions where AccountNumber = {CurrentCustomer.AccountNumber}";

                    // set the SQL Command string on the connection
                    // in a new connection object
                    SqlCommand myCommand = new SqlCommand(sql, connection);

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // run the Delete command - use command.ExecuteNonQuery()
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            Exception error = new Exception("No record matching that AccountNumber", ex);
                            throw error;
                        }
                        //Remove the contact from the ArrayList
                        CustomerList.Remove(CurrentCustomer);

                        //Remove the customer from the ListBox
                        CustomersListBox.Items.Remove(CurrentCustomer);
                        LoadCustomer();
                    }
                }
            }
        }
        //Method to Calculate the Balance and assign it to a textbox
        public void getBalance()
        {
            //variable balance, exclusive to this method
            decimal Balance;
            //if current customer is not null, proceed
            if (CurrentCustomer != null)
            {
                //declare a connection object
                using (SqlConnection connection = new SqlConnection())
                {
                    //point connection to database
                    connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\CustomerAccounts.mdf;Integrated Security=True";
                    //open the connection
                    connection.Open();

                    // Create a SQL command object, get the sum of the data by the account number selected
                    //if there is no data to sum, assign it a 0 value
                    // setup a SQL Command  - here SELECT
                    string sql = $"SELECT COALESCE(SUM(TransactionAmount),0) From AccountTransactions Where AccountNumber = {CurrentCustomer.AccountNumber}";
                    // set the SQL Command string on the connection
                    // in a new connection object
                    SqlCommand myCommand = new SqlCommand(sql, connection);

                    // Obtain a data reader from ExecuteReader().
                    using (SqlDataReader myDataReader = myCommand.ExecuteReader())
                    {
                        // Loop over the results.
                        while (myDataReader.Read())
                        {
                            //store the results into the balance variable
                            Balance = myDataReader.GetDecimal(0);
                            //display results in the Balance textbox
                            txbBalance.Text = Convert.ToString(Balance);
                        }
                    }
                }
            }
        }
        //Display the record for the Customer selected in the listbox
        public void DisplayCustomerRecord(CustomerAccount CustomerItem)
        {
            //proceed only if there is a selected item in the Customer Listbox
            if (CustomersListBox.SelectedItem != null)
            {
                //populate the textboxes with the selected data
                txbFirstName.Text = CustomerItem.FirstName;
                txbLastName.Text = CustomerItem.LastName;
                txbAccount.Text = Convert.ToString(CustomerItem.AccountNumber);
                txbPhone.Text = CustomerItem.Phone;
                txbEmail.Text = CustomerItem.Email;
            }
            else
            {
                // empty the form
                txbFirstName.Text = "";
                txbLastName.Text = "";
                txbAccount.Text = "";
                txbBalance.Text = "";
                txbEmail.Text = "";
                txbPhone.Text = "";
            }
            //clear the transaction listbox
            TransactionsListBox.Items.Clear();
            //display the transactions
            DisplayTransactionRecord();
        }
        //Display Transactions for the Customer selected
        public void DisplayTransactionRecord()
        {
            //variable
            int value = 0;
            //proceed only if therre is no selected item in the transaction lisbox and the customer listbox has a selected item
            if (TransactionsListBox.SelectedItem == null && CustomersListBox.SelectedItem != null)
            {
                //clear transaction listbox
                TransactionsListBox.Items.Clear();
                //loop and populate the listbox
                for (value = 0; value < TransactionList.Count; value++)
                {
                    CurrentCustomer = (CustomerAccount)CustomersListBox.SelectedItem;
                    CurrentTransaction = TransactionList[value];
                    if (CurrentCustomer.AccountNumber == CurrentTransaction.AccountNumber)
                    {
                        TransactionsListBox.Items.Add(CurrentTransaction);
                    }
                }
            }
            //current transaction is the slected item in the lisbox
            CurrentTransaction = (AccountTransaction)TransactionsListBox.SelectedItem;
            //if there is a selected item in the transaction listbox, proceed
            if (TransactionsListBox.SelectedItem != null)
            {
                //assign the selected data to the textboxes
                txbTransactionAmount.Text = Convert.ToString(CurrentTransaction.TransactionAmount);
                txbTransactionDate.Text = Convert.ToString(CurrentTransaction.TransactionDate);
                txbTransactionNumber.Text = Convert.ToString(CurrentTransaction.TransactionNumber);
            }

            else
            {
                //empty the form
                txbTransactionAmount.Text = "";
                txbTransactionDate.Text = "";
                txbTransactionNumber.Text = "";
            }
            //get the balance
            getBalance();
        }
        private void CustomersListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            //Get the currently selected Object in the listbox, as a customer
            CurrentCustomer = (CustomerAccount)CustomersListBox.SelectedItem;

            // now display if possible
            DisplayCustomerRecord(CurrentCustomer);
        }

        private void TransactionsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // get the currently selected object in the lisbox as a transaction
            CurrentTransaction = (AccountTransaction)TransactionsListBox.SelectedItem;
            //now display if possible
            DisplayTransactionRecord();
        }
    }
}