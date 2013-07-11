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
    }
}
