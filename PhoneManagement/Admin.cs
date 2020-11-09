using PhoneManagement.DAO;
using PhoneManagement.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhoneManagement
{
    public partial class frmAdmin : Form
    {
        BindingSource listPhone = new BindingSource(); //tạo một bindingsource toàn cục để gán vào, khi binding không bị thay đổi
        BindingSource accountList = new BindingSource();
        public AccountDTO loginAccount;
        public frmAdmin()
        {

            InitializeComponent();
            dtgvShowPhone.DataSource = listPhone;
            dtgvAccount.DataSource = accountList;
            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFromDate.Value);
            LoadListPhone();
            LoadCategoryinPhoneControl(cbxPhoneCategory);
            LoadAccount();
            AddPhoneBinding();
            AddAcountBinding();
        }

        #region Methods
        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);

        }

        void LoadListBillByDate(DateTime dateCheckIn)
        {
            dtgvTarget.DataSource = BillDAO.Instance.GetBillListByDate(dateCheckIn);
        }

        void LoadListPhone()
        {
            listPhone.DataSource = TableDao.Instance.GetListPhone1();
        }

        void LoadCategoryinPhoneControl(ComboBox cb)
        {
            cb.DataSource = PhoneCategoryDAO.Instance.LoadTableListPhoneCate();
            cb.DisplayMember = "Name";
        }

        void AddPhoneBinding()
        {
            txtPhoneName.DataBindings.Add(new Binding("Text", dtgvShowPhone.DataSource, "Name", true, DataSourceUpdateMode.Never)); // true chấp nhận ép kiểu, DataSourceUpdateMode.Never: không bao giờ thay đổi giá trị khi load
            txtPhoneID.DataBindings.Add(new Binding("Text", dtgvShowPhone.DataSource, "IdPhone", true, DataSourceUpdateMode.Never));
            txtPhonePrice.DataBindings.Add(new Binding("Text", dtgvShowPhone.DataSource, "Price", true, DataSourceUpdateMode.Never));
            txtIdItemPhone.DataBindings.Add(new Binding("Text", dtgvShowPhone.DataSource, "idItemPhone", true, DataSourceUpdateMode.Never));
            // kĩ thuật binding, thằng này theo đổi, thằng sau thay dổi theo
            //Từ txbFoodName lấy giá trị text, thuộc tính của text sẽ thay đổi theo giá trị của Name nằm trong DataSource

        }
        List<Table> SearchFoodByName(string name)
        {
            List<Table> listPhoneSearch = TableDao.Instance.SearchPhoneByName(name);

            return listPhoneSearch;
        }
        void AddAcountBinding()
        {
            txtUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txtDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            nmrAccountType.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }

        void LoadAccount()
        {
            accountList.DataSource = AccountDao.Instance.GetListAccount();
        }

        void AddAccount(string userName, string displayName, int type)
        {
            if (AccountDao.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Account added successfully");
            }
            else
            {
                MessageBox.Show("Account add failed");
            }

            LoadAccount();
        }

        void EditAccount(string userName, string displayName, int type)
        {
            if (AccountDao.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Successful account update");
            }
            else
            {
                MessageBox.Show("Account update failed");
            }

            LoadAccount();
        }

        void DeleteAccount(string userName)
        {
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Please don't delete yourself");
                return;
            }
            if (AccountDao.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Account deleted successfully");
            }
            else
            {
                MessageBox.Show("Account deletion failed");
            }

            LoadAccount();
        }

        void ResetPass(string userName)
        {
            if (AccountDao.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Password reset was successful");
            }
            else
            {
                MessageBox.Show("Password reset failed");
            }
        }

        #endregion

        #region Events
        private void button1_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value);
        }



        private void btnWatch_Click(object sender, EventArgs e)
        {
            LoadListPhone();
        }

        private void txtPhoneID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvShowPhone.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvShowPhone.SelectedCells[0].OwningRow.Cells["IdPhone"].Value;

                    PhoneCategoryDTO category = PhoneCategoryDAO.Instance.GetCategoryByIdPhone(id);

                    cbxPhoneCategory.SelectedItem = category;

                    int index = -1;
                    int i = 0;
                    foreach (PhoneCategoryDTO item in cbxPhoneCategory.Items)
                    {
                        if (item.IdPhone == category.IdPhone)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }

                    cbxPhoneCategory.SelectedIndex = index;
                }
            }
            catch { }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name = txtPhoneName.Text;
            int idPhone = (cbxPhoneCategory.SelectedItem as PhoneCategoryDTO).IdPhone;
            //int idPhone = Int32.Parse(txtPhoneID.Text);
            float price = float.Parse(txtPhonePrice.Text);

            if (TableDao.Instance.InsertPhone(idPhone, name, price))
            {
                MessageBox.Show("Add successfully");
                LoadListPhone();
                if (insertPhone != null)
                    insertPhone(this, new EventArgs());
            }

            else
            {
                MessageBox.Show("Add Failed");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            string name = txtPhoneName.Text;
            int idPhone = (cbxPhoneCategory.SelectedItem as PhoneCategoryDTO).IdPhone;
            ///int idPhone = Int32.Parse(txtPhoneID.Text);
            float price = float.Parse(txtPhonePrice.Text);
            int idItemPhone = Int32.Parse(txtIdItemPhone.Text);


            if (TableDao.Instance.UpdatePhone(idPhone, idItemPhone,name, price))
            {
                MessageBox.Show("Update successfully");
                LoadListPhone();
                if (updatePhone != null)
                    updatePhone(this, new EventArgs());
            }

            else
            {
                MessageBox.Show("Update Failed");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int idItemPhone = Int32.Parse(txtIdItemPhone.Text);


            if (TableDao.Instance.DeletePhone(idItemPhone))
            {
                MessageBox.Show("Update successfully");
                LoadListPhone();
                if (deletePhone != null)
                    deletePhone(this, new EventArgs());
            }

            else
            {
                MessageBox.Show("Update Failed");
            }
        }

        private event EventHandler insertPhone;
        public event EventHandler InsertPhone
        {
            add { insertPhone += value; }
            remove { insertPhone -= value; }
        }

        private event EventHandler deletePhone;
        public event EventHandler DeletePhone
        {
            add { deletePhone += value; }
            remove { deletePhone -= value; }
        }

        private event EventHandler updatePhone;
        public event EventHandler UpdatePhone
        {
            add { updatePhone += value; }
            remove { updatePhone -= value; }
        }

        private void btnPhoneSearch_Click(object sender, EventArgs e)
        {
            listPhone.DataSource = SearchFoodByName(txtPhoneSearch.Text);
        }

        private void bntShowAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        private void btnAddAdcount_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string displayName = txtDisplayName.Text;
            int type = (int)nmrAccountType.Value;

            AddAccount(userName, displayName, type);
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;

            DeleteAccount(userName);
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string displayName = txtDisplayName.Text;
            int type = (int)nmrAccountType.Value;

            EditAccount(userName, displayName, type);
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;

            ResetPass(userName);
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            txtPageBill.Text = "1";
        }

        private void btnPevious_Click(object sender, EventArgs e)
        {

            int page = Convert.ToInt32(txtPageBill.Text);

            if (page > 1)
                page--;

            txtPageBill.Text = page.ToString();
        }

        private void txtPageBill_TextChanged(object sender, EventArgs e)
        {
            dtgvTarget.DataSource = BillDAO.Instance.GetBillListByDateAndPage(dtpkFromDate.Value, Convert.ToInt32(txtPageBill.Text));
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txtPageBill.Text);
            int sumRecord = BillDAO.Instance.GetNumBillListByDate(dtpkFromDate.Value);

            if (page < sumRecord)
                page++;

            txtPageBill.Text = page.ToString();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            int sumRecord = BillDAO.Instance.GetNumBillListByDate(dtpkFromDate.Value);

            int lastPage = sumRecord / 10;

            if (sumRecord % 10 != 0)
                lastPage++;

            txtPageBill.Text = lastPage.ToString();
        }

        private void dtgvTarget_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        #endregion
    }
}
