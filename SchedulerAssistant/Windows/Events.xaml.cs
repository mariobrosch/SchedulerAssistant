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
    /// Interaction logic for Events.xaml
    /// </summary>
    public partial class Events : Window
    {
        public Events()
        {
            InitializeComponent();

            LoadBoxes();
        }

        private List<Event> events;
        private List<int> invitedContacts;
        private List<string> invitedContactTypes;
        private List<Contact> allContacts;

        private void LoadBoxes()
        {
            events = EventData.Get(ChkShowRemoved.IsChecked.GetValueOrDefault())?.OrderBy(p => p.DisplayValue).ToList() ?? new List<Event>();

            allContacts = ContactData.Get().OrderBy(p => p.DisplayValue).ToList() ?? new List<Contact>();

            cbLocation.Items.Clear();
            foreach (string name in Enum.GetNames(typeof(Location)))
            {
                cbLocation.Items.Add(name);
            }

            LbEvents.ItemsSource = events;
            LbEvents.DisplayMemberPath = "DisplayValue";

            if (events.Count == 0)
            {
                BtnNew_Click(null, null);
            }
            else
            {
                LbEvents.SelectedIndex = 0;
            }
        }

        private void ChkShowRemoved_Click(object sender, RoutedEventArgs e)
        {
            LoadBoxes();
        }

        private void BtnNew_Click(object? sender, RoutedEventArgs? e)
        {
            LbEvents.UnselectAll();
            txtName.Text = "";
            txtDescription.Text = "";
            txtId.Text = "Nieuw";
            txtTime.Text = "";
            cbLocation.SelectedIndex = 0;
            BtnRemove.Visibility = Visibility.Hidden;
            ChkModeramenOnly.IsChecked = false;
            dtDate.SelectedDate = DateTime.Now;
            invitedContacts = new List<int>();
            invitedContactTypes = new List<string>();

        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Verwijder event?", "Verwijder?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Event @event = (Event)LbEvents.SelectedItem;
                @event.IsRemoved = true;
                _ = EventData.Update(@event);
                LoadBoxes();
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var selectedLocation = cbLocation.SelectedItem;
            _ = Enum.TryParse(selectedLocation.ToString(), out Location myLocation);

            if (txtId.Text != "Nieuw")
            {
                Event @event = (Event)LbEvents.SelectedItem;
                if (@event == null)
                {
                    return;
                }
                @event.IsRemoved = ChkIsRemoved.IsChecked.GetValueOrDefault();
                @event.Name = txtName.Text;
                @event.Description = txtDescription.Text;
                @event.Time = txtTime.Text;
                @event.Location = myLocation.ToString();
                @event.Date = dtDate.ToString();
                @event.ModeramenOnly = ChkModeramenOnly.IsChecked.GetValueOrDefault();
                @event.LastModifiedDateTime = DateTime.Now;
                @event.InvitedContacts = invitedContacts;
                @event.InvitedContactTypes = invitedContactTypes;

                _ = EventData.Update(@event);
            }
            else
            {

                Event newEvent = new()
                {
                    IsRemoved = ChkIsRemoved.IsChecked.GetValueOrDefault(),
                    Name = txtName.Text,
                    Description = txtDescription.Text,
                    Time = txtTime.Text,
                    Location = myLocation.ToString(),
                    Date = dtDate.ToString(),
                    ModeramenOnly = ChkModeramenOnly.IsChecked.GetValueOrDefault(),
                    LastModifiedDateTime = DateTime.Now,
                    InvitedContacts = invitedContacts,
                    InvitedContactTypes = invitedContactTypes
                };

                _ = EventData.Create(newEvent);
            }
            LoadBoxes();
        }

        private void LbEvents_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((Event)LbEvents.SelectedItem != null)
            {
                LoadEvent((Event)LbEvents.SelectedItem);
            }
        }

        private void LoadEvent(Event @event)
        {
            ChkIsRemoved.IsChecked = @event.IsRemoved;
            txtName.Text = @event.Name;
            txtDescription.Text = @event.Description;
            txtTime.Text = @event.Time;
            cbLocation.SelectedItem = @event.Location;
            dtDate.SelectedDate = DateTime.Parse(@event.Date ?? DateTime.Now.ToString());
            ChkModeramenOnly.IsChecked = @event.ModeramenOnly;
            LblInvitedContacts.Text = GetInvitedContacts(@event.InvitedContacts);
            LblInvitedContactTypes.Text = String.Join(", ", @event.InvitedContactTypes ?? new List<string>());
            BtnRemove.Visibility = Visibility.Visible;
        }

        private string GetInvitedContacts(List<int>? invitedContacts)
        {
            var returnValue = new List<string>();

            foreach(var contact in invitedContacts)
            {
                returnValue.Add(allContacts.First(c => c.Id == contact)?.DisplayValue ?? "unknown");
            }

            return String.Join(", ", returnValue);
        }

        private void LbEvents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((Event)LbEvents.SelectedItem != null)
            {
                LoadEvent((Event)LbEvents.SelectedItem);
            }
        }

        private void ChkIsActive_Click(object sender, RoutedEventArgs e)
        {
            LoadBoxes();
        }
    }
}
