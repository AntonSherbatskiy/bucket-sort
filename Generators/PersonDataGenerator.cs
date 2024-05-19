using Bogus;
using ParallelBucketSort.Generators.Core;
using Models_Person = ParallelBucketSort.Models.Person;
using Person = ParallelBucketSort.Models.Person;

namespace ParallelBucketSort.Generators;

public class PersonDataGenerator : IDataGenerator<Models_Person>
{
    private Faker<Models_Person> _personFaker { get; set; }

    public PersonDataGenerator()
    {
        _personFaker = new Faker<Models_Person>()
            .RuleFor(p => p.FirstName, faker => faker.Name.FirstName())
            .RuleFor(p => p.LastName, faker => faker.Name.LastName())
            .RuleFor(p => p.Email, (faker, person) => faker.Internet.Email(person.FirstName, person.LastName))
            .RuleFor(p => p.Age, faker => faker.Random.Int(1, 100));
    }

    public List<Models_Person> Generate(int elementsCount)
    {
        return _personFaker.GenerateForever().Take(elementsCount).ToList();
    }
}