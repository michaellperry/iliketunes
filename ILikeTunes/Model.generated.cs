using System.Collections.Generic;
using System.Linq;
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
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"ILikeTunes.Individual", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles

        // Queries
        public static Query MakeQueryName()
		{
			return new Query()
				.JoinSuccessors(Individual__name.RoleIndividual, Condition.WhereIsEmpty(Individual__name.MakeQueryIsCurrent())
				)
            ;
		}
        public static Query QueryName = MakeQueryName();
        public static Query MakeQueryTunes()
		{
			return new Query()
				.JoinSuccessors(Like.RoleIndividual)
				.JoinPredecessors(Like.RoleTune)
            ;
		}
        public static Query QueryTunes = MakeQueryTunes();

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
            _name = new Result<Individual__name>(this, QueryName);
            _tunes = new Result<Tune>(this, QueryTunes);
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
				var current = _name.Ensure().ToList();
				if (current.Count != 1 || !object.Equals(current[0].Value, value.Value))
				{
					Community.AddFact(new Individual__name(this, _name, value.Value));
				}
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
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"ILikeTunes.Individual__name", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RoleIndividual = new Role(new RoleMemento(
			_correspondenceFactType,
			"individual",
			new CorrespondenceFactType("ILikeTunes.Individual", 1),
			true));
        public static Role RolePrior = new Role(new RoleMemento(
			_correspondenceFactType,
			"prior",
			new CorrespondenceFactType("ILikeTunes.Individual__name", 1),
			false));

        // Queries
        public static Query MakeQueryIsCurrent()
		{
			return new Query()
				.JoinSuccessors(Individual__name.RolePrior)
            ;
		}
        public static Query QueryIsCurrent = MakeQueryIsCurrent();

        // Predicates
        public static Condition IsCurrent = Condition.WhereIsEmpty(QueryIsCurrent);

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
            _individual = new PredecessorObj<Individual>(this, RoleIndividual, individual);
            _prior = new PredecessorList<Individual__name>(this, RolePrior, prior);
            _value = value;
        }

        // Hydration constructor
        private Individual__name(FactMemento memento)
        {
            InitializeResults();
            _individual = new PredecessorObj<Individual>(this, RoleIndividual, memento);
            _prior = new PredecessorList<Individual__name>(this, RolePrior, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public Individual Individual
        {
            get { return _individual.Fact; }
        }
        public IEnumerable<Individual__name> Prior
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
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"ILikeTunes.Tune", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles

        // Queries
        public static Query MakeQueryIndividuals()
		{
			return new Query()
				.JoinSuccessors(Like.RoleTune)
				.JoinPredecessors(Like.RoleIndividual)
            ;
		}
        public static Query QueryIndividuals = MakeQueryIndividuals();

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
            _individuals = new Result<Individual>(this, QueryIndividuals);
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
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"ILikeTunes.Like", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RoleIndividual = new Role(new RoleMemento(
			_correspondenceFactType,
			"individual",
			new CorrespondenceFactType("ILikeTunes.Individual", 1),
			true));
        public static Role RoleTune = new Role(new RoleMemento(
			_correspondenceFactType,
			"tune",
			new CorrespondenceFactType("ILikeTunes.Tune", 1),
			true));

        // Queries

        // Predicates

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
            _individual = new PredecessorObj<Individual>(this, RoleIndividual, individual);
            _tune = new PredecessorObj<Tune>(this, RoleTune, tune);
        }

        // Hydration constructor
        private Like(FactMemento memento)
        {
            InitializeResults();
            _individual = new PredecessorObj<Individual>(this, RoleIndividual, memento);
            _tune = new PredecessorObj<Tune>(this, RoleTune, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public Individual Individual
        {
            get { return _individual.Fact; }
        }
        public Tune Tune
        {
            get { return _tune.Fact; }
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
				Individual.QueryName.QueryDefinition);
			community.AddQuery(
				Individual._correspondenceFactType,
				Individual.QueryTunes.QueryDefinition);
			community.AddType(
				Individual__name._correspondenceFactType,
				new Individual__name.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Individual__name._correspondenceFactType }));
			community.AddQuery(
				Individual__name._correspondenceFactType,
				Individual__name.QueryIsCurrent.QueryDefinition);
			community.AddType(
				Tune._correspondenceFactType,
				new Tune.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Tune._correspondenceFactType }));
			community.AddQuery(
				Tune._correspondenceFactType,
				Tune.QueryIndividuals.QueryDefinition);
			community.AddType(
				Like._correspondenceFactType,
				new Like.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Like._correspondenceFactType }));
		}
	}
}
