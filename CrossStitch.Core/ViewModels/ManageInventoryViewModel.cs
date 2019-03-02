using CrossStitch.Core.Attributes;
using CrossStitch.Core.Data;
using CrossStitch.Core.Extensions;
using CrossStitch.Core.Interfaces;
using CrossStitch.Core.Models;
using CrossStitch.Core.Models.SearchCriteria;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace CrossStitch.Core.ViewModels
{
    [Title("Manage Thread References")]
    public class ManageInventoryViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;
        private readonly ThreadReferenceRepository _threadReferenceRepository;
        private ObservableCollection<ThreadReference> _threadReferences = new ObservableCollection<ThreadReference>();

        public ManageInventoryViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            ThreadReferenceRepository threadReferenceRepository)
        {
            _dialogService = dialogService;
            _threadReferenceRepository = threadReferenceRepository;

            SaveCommand = new RelayCommand(OnSave, CanSave);
            SearchCommand = new RelayCommand(OnSearch, CanSearch);
            NewThreadReferenceCommand = new RelayCommand(OnNewThreadReference);
            EditThreadReferenceCommand = new RelayCommand<ThreadReference>(OnEditThreadReference);
            DeleteThreadReferenceCommand = new RelayCommand<ThreadReference>(OnDeleteThreadReference);
        }

        public InventorySearchCriteria Criteria { get; set; } = new InventorySearchCriteria();

        public ICommand DeleteThreadReferenceCommand { get; }

        public ICommand EditThreadReferenceCommand { get; }

        public ICommand NewThreadReferenceCommand { get; }

        public ObservableCollection<ThreadReference> OwnedThreads => _threadReferences.Where(tr => tr.Owned).AsObservable();

        public ICommand SaveCommand { get; }

        public ICommand SearchCommand { get; }

        public ObservableCollection<WrappedThreadReference> SearchResults { get; set; } = new ObservableCollection<WrappedThreadReference>();

        public ThreadReference SelectedItem { get; set; }

        public override void Initialise(object param)
        {
            Criteria = new InventorySearchCriteria();
            SearchResults = new ObservableCollection<WrappedThreadReference>();
            _threadReferences = _threadReferenceRepository.GetAll().OrderBy(o => !o.Owned).ThenBy(o => o.Id).AsObservable();
        }

        public override void OnBack()
        {
            OnSearch();
        }

        private bool CanSave()
        {
            return SearchResults.Any(s => s.NewLength != null);
        }

        private bool CanSearch() => Criteria.IsValid;

        private void EditThreadReference(ThreadReference threadReference)
        {
            if (!RemoveExistingUpdates())
                return;

            Navigate<EditThreadReferenceViewModel>(threadReference);
        }

        private bool HasUpdatedThreads()
        {
            return SearchResults.Any(sr => sr.NewLength != null);
        }

        private void OnDeleteThreadReference(ThreadReference obj)
        {
            throw new NotImplementedException();
        }

        private void OnEditThreadReference(ThreadReference threadReference)
        {
            var thread = _threadReferenceRepository.Get(threadReference.Id);
            EditThreadReference(thread);
        }

        private void OnNewThreadReference()
        {
            EditThreadReference(new ThreadReference());
        }

        private void OnSave()
        {
            var referencesToSave = SearchResults
                .Where(sr => sr.NewLength != null)
                .Select(sr =>
                {
                    var reference = sr.Wrapped;
                    reference.OwnedLength = sr.NewLength.GetValueOrDefault();
                    return reference;
                })
                .ToArray();

            _threadReferenceRepository.Save(referencesToSave);

            SearchResults = SearchResults.Select(x =>
            {
                x.NewLength = null;
                return x;
            }).AsObservable();
            OnSearch();
        }

        private void OnSearch()
        {
            if (!RemoveExistingUpdates())
                return;

            SearchResults = _threadReferenceRepository
                .Query(Criteria)
                .OrderBy(o => !o.Owned)
                .ThenBy(o => o.BrandName)
                .ThenBy(o => o.BrandId)
                .Select(x => new WrappedThreadReference(x))
                .AsObservable();
        }

        private bool RemoveExistingUpdates()
        {
            if (!HasUpdatedThreads())
                return true;

            return _dialogService.ShowQuestion("Current length updates will be discarded. Continue?", "Has Updated Threads");
        }
    }

    public class WrappedThreadReference : WrappedEntity<ThreadReference>
    {
        public WrappedThreadReference(ThreadReference wrapped)
            : base(wrapped)
        { }

        public string BrandId => Wrapped.BrandId;
        public string BrandName => Wrapped.BrandName;
        public string Colour => Wrapped.Colour;
        public int CurrentLength => Wrapped.OwnedLength;
        public string Description => Wrapped.Description;
        public int? NewLength { get; set; }
    }
}