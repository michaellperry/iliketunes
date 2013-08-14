using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpdateControls.Correspondence;
using UpdateControls.Correspondence.Mementos;
using UpdateControls.Correspondence.Strategy;
using System;
using System.IO;

/**
/ For use with http://graphviz.org/
digraph "ILikeTunes"
{
    rankdir=BT
    Individual__name -> Individual [color="red"]
    Individual__name -> Individual__name [label="  *"]
    Like -> Individual [color="red"]
    Like -> Tune [color="red"]
    Dislike -> Like
}
**/

namespace ILikeTunes
{
    public partial class Individual : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Individual newFact = new Individual(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._anonymousId = (string)_fieldSerializerByType[typeof(string)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Individual fact = (Individual)obj;
				_fieldSerializerByType[typeof(string)].WriteData(output, fact._anonymousId);
			}

            public CorrespondenceFact GetUnloadedInstance()
            {
                return Individual.GetUnloadedInstance();
            }

            public CorrespondenceFact GetNullInstance()
            {
                return Individual.GetNullInstance();
            }
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"ILikeTunes.Individual", 8);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Null and unloaded instances
        public static Individual GetUnloadedInstance()
        {
            return new Individual((FactMemento)null) { IsLoaded = false };
        }

        public static Individual GetNullInstance()
        {
            return new Individual((FactMemento)null) { IsNull = true };
        }

        // Ensure
        public Task<Individual> EnsureAsync()
        {
            if (_loadedTask != null)
                return _loadedTask.ContinueWith(t => (Individual)t.Result);
            else
                return Task.FromResult(this);
        }

        // Roles

        // Queries
        private static Query _cacheQueryName;

        public static Query GetQueryName()
		{
            if (_cacheQueryName == null)
            {
			    _cacheQueryName = new Query()
    				.JoinSuccessors(Individual__name.GetRoleIndividual(), Condition.WhereIsEmpty(Individual__name.GetQueryIsCurrent())
				)
                ;
            }
            return _cacheQueryName;
		}
        private static Query _cacheQueryTunes;

        public static Query GetQueryTunes()
		{
            if (_cacheQueryTunes == null)
            {
			    _cacheQueryTunes = new Query()
    				.JoinSuccessors(Like.GetRoleIndividual(), Condition.WhereIsEmpty(Like.GetQueryIsCurrent())
				)
		    		.JoinPredecessors(Like.GetRoleTune())
                ;
            }
            return _cacheQueryTunes;
		}

        // Predicates

        // Predecessors

        // Fields
        private string _anonymousId;

        // Results
        private Result<Individual__name> _name;
        private Result<Tune> _tunes;

        // Business constructor
        public Individual(
            string anonymousId
            )
        {
            InitializeResults();
            _anonymousId = anonymousId;
        }

        // Hydration constructor
        private Individual(FactMemento memento)
        {
            InitializeResults();
        }

        // Result initializer
        private void InitializeResults()
        {
            _name = new Result<Individual__name>(this, GetQueryName(), Individual__name.GetUnloadedInstance, Individual__name.GetNullInstance);
            _tunes = new Result<Tune>(this, GetQueryTunes(), Tune.GetUnloadedInstance, Tune.GetNullInstance);
        }

        // Predecessor access

        // Field access
        public string AnonymousId
        {
            get { return _anonymousId; }
        }

        // Query result access
        public Result<Tune> Tunes
        {
            get { return _tunes; }
        }

        // Mutable property access
        public TransientDisputable<Individual__name, string> Name
        {
            get { return _name.AsTransientDisputable(fact => fact.Value); }
			set
			{
                Action setter = async delegate()
                {
                    var current = (await _name.EnsureAsync()).ToList();
                    if (current.Count != 1 || !object.Equals(current[0].Value, value.Value))
                    {
                        await Community.AddFactAsync(new Individual__name(this, _name, value.Value));
                    }
                };
                setter();
			}
        }

    }
    
    public partial class Individual__name : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Individual__name newFact = new Individual__name(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._value = (string)_fieldSerializerByType[typeof(string)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Individual__name fact = (Individual__name)obj;
				_fieldSerializerByType[typeof(string)].WriteData(output, fact._value);
			}

            public CorrespondenceFact GetUnloadedInstance()
            {
                return Individual__name.GetUnloadedInstance();
            }

            public CorrespondenceFact GetNullInstance()
            {
                return Individual__name.GetNullInstance();
            }
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"ILikeTunes.Individual__name", 1335857788);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Null and unloaded instances
        public static Individual__name GetUnloadedInstance()
        {
            return new Individual__name((FactMemento)null) { IsLoaded = false };
        }

        public static Individual__name GetNullInstance()
        {
            return new Individual__name((FactMemento)null) { IsNull = true };
        }

        // Ensure
        public Task<Individual__name> EnsureAsync()
        {
            if (_loadedTask != null)
                return _loadedTask.ContinueWith(t => (Individual__name)t.Result);
            else
                return Task.FromResult(this);
        }

        // Roles
        private static Role _cacheRoleIndividual;
        public static Role GetRoleIndividual()
        {
            if (_cacheRoleIndividual == null)
            {
                _cacheRoleIndividual = new Role(new RoleMemento(
			        _correspondenceFactType,
			        "individual",
			        Individual._correspondenceFactType,
			        true));
            }
            return _cacheRoleIndividual;
        }
        private static Role _cacheRolePrior;
        public static Role GetRolePrior()
        {
            if (_cacheRolePrior == null)
            {
                _cacheRolePrior = new Role(new RoleMemento(
			        _correspondenceFactType,
			        "prior",
			        Individual__name._correspondenceFactType,
			        false));
            }
            return _cacheRolePrior;
        }

        // Queries
        private static Query _cacheQueryIsCurrent;

        public static Query GetQueryIsCurrent()
		{
            if (_cacheQueryIsCurrent == null)
            {
			    _cacheQueryIsCurrent = new Query()
		    		.JoinSuccessors(Individual__name.GetRolePrior())
                ;
            }
            return _cacheQueryIsCurrent;
		}

        // Predicates
        public static Condition IsCurrent = Condition.WhereIsEmpty(GetQueryIsCurrent());

        // Predecessors
        private PredecessorObj<Individual> _individual;
        private PredecessorList<Individual__name> _prior;

        // Fields
        private string _value;

        // Results

        // Business constructor
        public Individual__name(
            Individual individual
            ,IEnumerable<Individual__name> prior
            ,string value
            )
        {
            InitializeResults();
            _individual = new PredecessorObj<Individual>(this, GetRoleIndividual(), individual);
            _prior = new PredecessorList<Individual__name>(this, GetRolePrior(), prior);
            _value = value;
        }

        // Hydration constructor
        private Individual__name(FactMemento memento)
        {
            InitializeResults();
            _individual = new PredecessorObj<Individual>(this, GetRoleIndividual(), memento, Individual.GetUnloadedInstance, Individual.GetNullInstance);
            _prior = new PredecessorList<Individual__name>(this, GetRolePrior(), memento, Individual__name.GetUnloadedInstance, Individual__name.GetNullInstance);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public Individual Individual
        {
            get { return IsNull ? Individual.GetNullInstance() : _individual.Fact; }
        }
        public PredecessorList<Individual__name> Prior
        {
            get { return _prior; }
        }

        // Field access
        public string Value
        {
            get { return _value; }
        }

        // Query result access

        // Mutable property access

    }
    
    public partial class Tune : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Tune newFact = new Tune(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._name = (string)_fieldSerializerByType[typeof(string)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Tune fact = (Tune)obj;
				_fieldSerializerByType[typeof(string)].WriteData(output, fact._name);
			}

            public CorrespondenceFact GetUnloadedInstance()
            {
                return Tune.GetUnloadedInstance();
            }

            public CorrespondenceFact GetNullInstance()
            {
                return Tune.GetNullInstance();
            }
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"ILikeTunes.Tune", 8);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Null and unloaded instances
        public static Tune GetUnloadedInstance()
        {
            return new Tune((FactMemento)null) { IsLoaded = false };
        }

        public static Tune GetNullInstance()
        {
            return new Tune((FactMemento)null) { IsNull = true };
        }

        // Ensure
        public Task<Tune> EnsureAsync()
        {
            if (_loadedTask != null)
                return _loadedTask.ContinueWith(t => (Tune)t.Result);
            else
                return Task.FromResult(this);
        }

        // Roles

        // Queries
        private static Query _cacheQueryIndividuals;

        public static Query GetQueryIndividuals()
		{
            if (_cacheQueryIndividuals == null)
            {
			    _cacheQueryIndividuals = new Query()
    				.JoinSuccessors(Like.GetRoleTune(), Condition.WhereIsEmpty(Like.GetQueryIsCurrent())
				)
		    		.JoinPredecessors(Like.GetRoleIndividual())
                ;
            }
            return _cacheQueryIndividuals;
		}

        // Predicates

        // Predecessors

        // Fields
        private string _name;

        // Results
        private Result<Individual> _individuals;

        // Business constructor
        public Tune(
            string name
            )
        {
            InitializeResults();
            _name = name;
        }

        // Hydration constructor
        private Tune(FactMemento memento)
        {
            InitializeResults();
        }

        // Result initializer
        private void InitializeResults()
        {
            _individuals = new Result<Individual>(this, GetQueryIndividuals(), Individual.GetUnloadedInstance, Individual.GetNullInstance);
        }

        // Predecessor access

        // Field access
        public string Name
        {
            get { return _name; }
        }

        // Query result access
        public Result<Individual> Individuals
        {
            get { return _individuals; }
        }

        // Mutable property access

    }
    
    public partial class Like : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Like newFact = new Like(memento);


				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Like fact = (Like)obj;
			}

            public CorrespondenceFact GetUnloadedInstance()
            {
                return Like.GetUnloadedInstance();
            }

            public CorrespondenceFact GetNullInstance()
            {
                return Like.GetNullInstance();
            }
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"ILikeTunes.Like", -304605072);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Null and unloaded instances
        public static Like GetUnloadedInstance()
        {
            return new Like((FactMemento)null) { IsLoaded = false };
        }

        public static Like GetNullInstance()
        {
            return new Like((FactMemento)null) { IsNull = true };
        }

        // Ensure
        public Task<Like> EnsureAsync()
        {
            if (_loadedTask != null)
                return _loadedTask.ContinueWith(t => (Like)t.Result);
            else
                return Task.FromResult(this);
        }

        // Roles
        private static Role _cacheRoleIndividual;
        public static Role GetRoleIndividual()
        {
            if (_cacheRoleIndividual == null)
            {
                _cacheRoleIndividual = new Role(new RoleMemento(
			        _correspondenceFactType,
			        "individual",
			        Individual._correspondenceFactType,
			        true));
            }
            return _cacheRoleIndividual;
        }
        private static Role _cacheRoleTune;
        public static Role GetRoleTune()
        {
            if (_cacheRoleTune == null)
            {
                _cacheRoleTune = new Role(new RoleMemento(
			        _correspondenceFactType,
			        "tune",
			        Tune._correspondenceFactType,
			        true));
            }
            return _cacheRoleTune;
        }

        // Queries
        private static Query _cacheQueryIsCurrent;

        public static Query GetQueryIsCurrent()
		{
            if (_cacheQueryIsCurrent == null)
            {
			    _cacheQueryIsCurrent = new Query()
		    		.JoinSuccessors(Dislike.GetRoleLike())
                ;
            }
            return _cacheQueryIsCurrent;
		}

        // Predicates
        public static Condition IsCurrent = Condition.WhereIsEmpty(GetQueryIsCurrent());

        // Predecessors
        private PredecessorObj<Individual> _individual;
        private PredecessorObj<Tune> _tune;

        // Fields

        // Results

        // Business constructor
        public Like(
            Individual individual
            ,Tune tune
            )
        {
            InitializeResults();
            _individual = new PredecessorObj<Individual>(this, GetRoleIndividual(), individual);
            _tune = new PredecessorObj<Tune>(this, GetRoleTune(), tune);
        }

        // Hydration constructor
        private Like(FactMemento memento)
        {
            InitializeResults();
            _individual = new PredecessorObj<Individual>(this, GetRoleIndividual(), memento, Individual.GetUnloadedInstance, Individual.GetNullInstance);
            _tune = new PredecessorObj<Tune>(this, GetRoleTune(), memento, Tune.GetUnloadedInstance, Tune.GetNullInstance);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public Individual Individual
        {
            get { return IsNull ? Individual.GetNullInstance() : _individual.Fact; }
        }
        public Tune Tune
        {
            get { return IsNull ? Tune.GetNullInstance() : _tune.Fact; }
        }

        // Field access

        // Query result access

        // Mutable property access

    }
    
    public partial class Dislike : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Dislike newFact = new Dislike(memento);


				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Dislike fact = (Dislike)obj;
			}

            public CorrespondenceFact GetUnloadedInstance()
            {
                return Dislike.GetUnloadedInstance();
            }

            public CorrespondenceFact GetNullInstance()
            {
                return Dislike.GetNullInstance();
            }
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"ILikeTunes.Dislike", 692981976);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Null and unloaded instances
        public static Dislike GetUnloadedInstance()
        {
            return new Dislike((FactMemento)null) { IsLoaded = false };
        }

        public static Dislike GetNullInstance()
        {
            return new Dislike((FactMemento)null) { IsNull = true };
        }

        // Ensure
        public Task<Dislike> EnsureAsync()
        {
            if (_loadedTask != null)
                return _loadedTask.ContinueWith(t => (Dislike)t.Result);
            else
                return Task.FromResult(this);
        }

        // Roles
        private static Role _cacheRoleLike;
        public static Role GetRoleLike()
        {
            if (_cacheRoleLike == null)
            {
                _cacheRoleLike = new Role(new RoleMemento(
			        _correspondenceFactType,
			        "like",
			        Like._correspondenceFactType,
			        false));
            }
            return _cacheRoleLike;
        }

        // Queries

        // Predicates

        // Predecessors
        private PredecessorObj<Like> _like;

        // Fields

        // Results

        // Business constructor
        public Dislike(
            Like like
            )
        {
            InitializeResults();
            _like = new PredecessorObj<Like>(this, GetRoleLike(), like);
        }

        // Hydration constructor
        private Dislike(FactMemento memento)
        {
            InitializeResults();
            _like = new PredecessorObj<Like>(this, GetRoleLike(), memento, Like.GetUnloadedInstance, Like.GetNullInstance);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public Like Like
        {
            get { return IsNull ? Like.GetNullInstance() : _like.Fact; }
        }

        // Field access

        // Query result access

        // Mutable property access

    }
    

	public class CorrespondenceModel : ICorrespondenceModel
	{
		public void RegisterAllFactTypes(Community community, IDictionary<Type, IFieldSerializer> fieldSerializerByType)
		{
			community.AddType(
				Individual._correspondenceFactType,
				new Individual.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Individual._correspondenceFactType }));
			community.AddQuery(
				Individual._correspondenceFactType,
				Individual.GetQueryName().QueryDefinition);
			community.AddQuery(
				Individual._correspondenceFactType,
				Individual.GetQueryTunes().QueryDefinition);
			community.AddType(
				Individual__name._correspondenceFactType,
				new Individual__name.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Individual__name._correspondenceFactType }));
			community.AddQuery(
				Individual__name._correspondenceFactType,
				Individual__name.GetQueryIsCurrent().QueryDefinition);
			community.AddType(
				Tune._correspondenceFactType,
				new Tune.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Tune._correspondenceFactType }));
			community.AddQuery(
				Tune._correspondenceFactType,
				Tune.GetQueryIndividuals().QueryDefinition);
			community.AddType(
				Like._correspondenceFactType,
				new Like.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Like._correspondenceFactType }));
			community.AddQuery(
				Like._correspondenceFactType,
				Like.GetQueryIsCurrent().QueryDefinition);
			community.AddType(
				Dislike._correspondenceFactType,
				new Dislike.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Dislike._correspondenceFactType }));
		}
	}
}
