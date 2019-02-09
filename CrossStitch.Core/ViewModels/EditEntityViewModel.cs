using CrossStitch.Core.Data;
using CrossStitch.Core.Interfaces;
using CrossStitch.Core.Models;
using CrossStitch.Core.Validation;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;

namespace CrossStitch.Core.ViewModels
{
    public abstract class EditEntityViewModel<T> : ViewModelBase where T : class, new()
    {
        public EditEntityViewModel()
        {
            SaveCommand = new RelayCommand(OnSave, CanSave);
        }

        public T Model { get; set; }
        public ICommand SaveCommand { get; }

        protected virtual bool CanSave() => true;

        protected abstract void OnSave();
    }

    public abstract class EditEntityWithThreadsViewModel<TEntity, TThread> : EditEntityViewModel<TEntity> where TEntity : class, IEntityWithThreads<TThread>, new()
                                                                                                          where TThread : ThreadBase
    {
        protected readonly IDialogService _dialogService;
        protected readonly ThreadReferenceRepository _threadReferenceRepository;
        protected readonly ThreadRepository _threadRepository;
        protected List<int> _initialIds;
        protected List<int> _threadIdsToRemove;

        public EditEntityWithThreadsViewModel(ThreadReferenceRepository threadReferenceRepository, ThreadRepository threadRepository, IDialogService dialogService)
        {
            _initialIds = new List<int>();
            _threadIdsToRemove = new List<int>();

            _threadReferenceRepository = threadReferenceRepository;
            _threadRepository = threadRepository;
            _dialogService = dialogService;
            AddThreadModel = new AddThreadModel();

            AddThreadCommand = new RelayCommand(OnAddThread, CanAddThread);
            DeleteThreadCommand = new RelayCommand<TThread>(OnDeleteThread, CanDeleteThread);
        }

        public ICommand AddThreadCommand { get; }
        public AddThreadModel AddThreadModel { get; set; }
        public ICommand DeleteThreadCommand { get; }

        public override void Initialise(object param)
        {
            Model = (param as TEntity) ?? new TEntity();
            AddThreadModel = new AddThreadModel();
            PopulateInitialIds();
        }

        protected virtual bool CanAddThread()
        {
            return CustomValidator.TryValidate(AddThreadModel);
        }

        protected virtual bool CanDeleteThread(TThread thread)
        {
            return true;
        }

        protected override bool CanSave()
        {
            return Model.Threads.All(t => t.Length.GetValueOrDefault() > 0);
        }

        protected abstract TThread GenerateThread(ThreadReference threadReference);

        protected virtual void OnAddThread()
        {
            if (Model.Threads.Any(o => o.ThreadReference.BrandId == AddThreadModel.BrandId
            && o.ThreadReference.BrandName == AddThreadModel.BrandName))
            {
                _dialogService.ShowInfo($"Thread {AddThreadModel.BrandName}-{AddThreadModel.BrandId} already added to the entity", "Duplicate Thread Reference");
                return;
            }

            var threadReference = _threadReferenceRepository
                .Query(o => o.BrandId == AddThreadModel.BrandId && o.BrandName == AddThreadModel.BrandName)
                .SingleOrDefault();

            if (threadReference == null)
            {
                _dialogService.ShowInfo($"Thread {AddThreadModel.BrandName}-{AddThreadModel.BrandId} could not be found in the inventory", "Missing Thread Reference");
                return;
            }

            AddThreadModel = new AddThreadModel();

            Model.Threads.Add(GenerateThread(threadReference));
        }

        protected virtual void OnDeleteThread(TThread thread)
        {
            Model.Threads.Remove(thread);
            _threadIdsToRemove.Add(thread.ThreadId);
        }

        protected override void OnSave()
        {
            var context = new ValidationContext(Model, null, null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(Model, context, results, true);

            if (results.Any())
            {
                var message = "Couldn't save the entity for the following reason(s):\n" + string.Join("\n", results);
                _dialogService.ShowWarning(message, "Validation Issues");
                return;
            }

            var remove = _initialIds.Intersect(_threadIdsToRemove);

            _threadRepository.Remove(remove.Select(id => new ThreadBase { ThreadId = id }).ToArray());
            SaveModel(Model);
            PopulateInitialIds();

            GoBack();
        }

        protected abstract void SaveModel(TEntity entity);

        private void PopulateInitialIds()
        {
            _initialIds = Model.Threads.Select(o => o.ThreadId).ToList();
        }
    }
}