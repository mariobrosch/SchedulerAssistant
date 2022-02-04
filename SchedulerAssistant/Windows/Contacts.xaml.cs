using SchedulerAssistant.Data.Data.Requests;
using SchedulerAssistant.Data.Enums;
using SchedulerAssistant.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SchedulerAssistant.Windows
{
    /// <summary>
    /// Interaction logic for Contacts.xaml
    /// </summary>
    public partial class Contacts : Window
    {
        public Contacts()
        {
            InitializeComponent();

            LoadBoxes();
        }
        
        private List<Contact> contacts;

        private void LoadBoxes()
        {
            contacts = ContactData.Get(ChkShowRemoved.IsChecked.GetValueOrDefault())?.OrderBy(p => p.DisplayValue).ToList() ?? new List<Contact>();
            if (!ChkActiveContact.IsChecked.GetValueOrDefault())
            {
                contacts = contacts.Where(t => t.IsEnabled).ToList();
            }

            lbType.Items.Clear();
            foreach (string name in Enum.GetNames(typeof(ContactType)))
            {
                lbType.Items.Add(name); 
            }

            LbContacts.ItemsSource = contacts;
            LbContacts.DisplayMemberPath = "DisplayValue";

            if (contacts.Count == 0)
            {
                BtnNew_Click(null, null);
            }
            else
            {
                LbContacts.SelectedIndex = 0;
            }
        }

        private void ChkShowRemoved_Click(object sender, RoutedEventArgs e)
        {
            LoadBoxes();
        }

        private void BtnNew_Click(object? sender, RoutedEventArgs? e)
        {
            LbContacts.UnselectAll();
            txtName.Text = "";
            txtAbbreviation.Text = "";
            txtId.Text = "Nieuw";
            txtLastName.Text = "";
            txtEmailaddress.Text = "";
            ChkActiveContact.IsChecked = true;
            BtnRemove.Visibility = Visibility.Hidden;
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Verwijder contact?", "Verwijder?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Contact contact = (Contact)LbContacts.SelectedItem;
                contact.IsRemoved = true;
                _ = ContactData.Update(contact);
                LoadBoxes();
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var selectedType = lbType.SelectedItem;
            _ = Enum.TryParse(selectedType.ToString(), out ContactType myType);

            if (txtId.Text != "Nieuw")
            {
                Contact contact = (Contact)LbContacts.SelectedItem;
                if (contact == null)
                {
                    return;
                }
                contact.IsEnabled = ChkActiveContact.IsChecked.GetValueOrDefault();
                contact.Name = txtName.Text;
                contact.Lastname = txtLastName.Text;
                contact.Type = myType;
                contact.EmailAddress = txtEmailaddress.Text;
                contact.Abbreviation = txtAbbreviation.Text;

                _ = ContactData.Update(contact);
            }
            else
            {

                Contact newContact = new()
                {
                    IsEnabled = ChkActiveContact.IsChecked.GetValueOrDefault(),
                    Name = txtName.Text,
                    Lastname = txtLastName.Text,
                    Type = myType,
                    EmailAddress = txtEmailaddress.Text,
                    Abbreviation = txtAbbreviation.Text,
                    IsRemoved = chkRemovedContact.IsChecked.GetValueOrDefault()
                };

                _ = ContactData.Create(newContact);
            }
            LoadBoxes();
        }

        private void LbContacts_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((Contact)LbContacts.SelectedItem != null)
            {
                LoadContact((Contact)LbContacts.SelectedItem);
            }
        }

        private void LoadContact(Contact contact)
        {
            txtId.Text = contact.Id.ToString();
            txtName.Text = contact.Name;
            txtLastName.Text = contact.Lastname;
            txtAbbreviation.Text = contact.Abbreviation;
            ChkActiveContact.IsChecked = contact.IsEnabled;
            chkRemovedContact.IsChecked = contact.IsRemoved;
            txtEmailaddress.Text = contact.EmailAddress;
            lbType.SelectedItem = contact.Type.ToString();
            BtnRemove.Visibility = Visibility.Visible;
        }

        private void LbContacts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((Contact)LbContacts.SelectedItem != null)
            {
                LoadContact((Contact)LbContacts.SelectedItem);
            }
        }

        private void ChkIsActive_Click(object sender, RoutedEventArgs e)
        {
            LoadBoxes();
        }
    }
}
