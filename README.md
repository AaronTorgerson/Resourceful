Resourceful
===========

A poorly-named .NET library to map your API models to dynamic RESTful resource representations.

- Add hypermedia links (haters gonna HATEOAS).
- Perform transformations that obviate the need for view models/dtos.

```csharp
// Say you've got a model, like
public class Thing
{
    public int Id { get; set; }
    public string Name { get; set; }
}
  
// Why go to the trouble of making a dto *just* to get hypermedia links? 
// Use Resourceful to take care of that for you.
ResourceMapper.CreateMapping<Thing>("/things/{Id}");

// In your action, map your model to its resource representation.
var model = new Thing 
{
    Id = 1, 
    Name = "Foo"
};

return model.AsResource();
```
And your json just got a little sexier:
```javascript
{
    'Id' : 1,
    'Name' : 'Foo',
    '_Relationships' : {
        'Self' : '/things/1'
    }
}
```
Wait a mintue, though... that Id clutters things up now that we have a nice Self link. Let's tweak our mapping.
```csharp
ResourceMapper.CreateMapping<Thing>("/things/{Id}")
              .OmitProperty("Id");
```
That's better:
```javascript
{
    'Name' : 'Foo',
    '_Relationships' : {
        'Self' : '/things/1'
    }
}
```
Now, how can we find Ribbits that have this Thing?
```csharp
ResourceMapper.CreateMapping<Thing>("/things/{Id}")
              .OmitProperty("Id")
              .AddRelationship("RibbitsWithThisThing", "/ribbits/?thingId={Id}");
```
Perfect.
```javascript
{
    'Name' : 'Foo',
    '_Relationships' : {
        'Self' : '/things/1',
        'RibbitsWithThisThing' : '/ribbits/?thingId=1'
    }
}
```
