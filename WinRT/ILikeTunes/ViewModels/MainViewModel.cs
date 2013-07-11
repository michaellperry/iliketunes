using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using UpdateControls.Correspondence;
using UpdateControls.Fields;
using UpdateControls.XAML;

namespace ILikeTunes.ViewModels
{
    public class MainViewModel
    {
        private Community _community;
        private Individual _individual;

        private Independent<string> _tuneName = new Independent<string>();
        private Independent<Tune> _selectedTune = new Independent<Tune>();
        private Independent<Individual> _selectedOtherIndividual = new Independent<Individual>();

        public MainViewModel(Community community, Individual individual)
        {
            _community = community;
            _individual = individual;
        }

        public bool Synchronizing
        {
            get { return _community.Synchronizing; }
        }

        public string LastException
        {
            get
            {
                return _community.LastException == null
                    ? String.Empty
                    : _community.LastException.Message;
            }
        }

        public string Name
        {
            get { return _individual.Name; }
            set { _individual.Name = value; }
        }

        public string TuneName
        {
            get { return _tuneName; }
            set { _tuneName.Value = value; }
        }

        public ICommand LikeTune
        {
            get
            {
                return MakeCommand
                    .Do(async delegate
                    {
                        Tune tune = await _community.AddFactAsync(new Tune(TuneName));
                        await _community.AddFactAsync(new Like(_individual, tune));
                        TuneName = null;
                    });
            }
        }

        public IEnumerable<Tune> Tunes
        {
            get { return _individual.Tunes; }
        }

        public Tune SelectedTune
        {
            get { return _selectedTune; }
            set { _selectedTune.Value = value; }
        }

        public IEnumerable<Individual> OtherIndividuals
        {
            get
            {
                if (_selectedTune.Value == null)
                    return new List<Individual>();

                return _selectedTune.Value.Individuals;
            }
        }

        public Individual SelectedOtherIndividual
        {
            get { return _selectedOtherIndividual.Value; }
            set { _selectedOtherIndividual.Value = value; }
        }

        public string OtherTunesCaption
        {
            get
            {
                if (_selectedOtherIndividual.Value != null)
                    return _selectedOtherIndividual.Value.Name.Value + " also likes:";
                else
                    return null;
            }
        }

        public IEnumerable<Tune> OtherTunes
        {
            get
            {
                if (_selectedOtherIndividual.Value != null)
                    return _selectedOtherIndividual.Value.Tunes;
                else
                    return new List<Tune>();
            }
        }
    }
}
