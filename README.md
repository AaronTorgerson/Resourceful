Resourceful
===========

A poorly-named library to help you serialize your models with hypermedia URIs.

Fluently map your .NET model classes to REST resource representations, 
* adding hypermedia links (haters gonna HATEOAS)
* performing transformations that obviate the need for view models/dtos, including omitting properties, such as IDs and child collections

'''
public class MyResource
{
    public int Id { get; set; }
    	public string Name { get; set; }
    }
  
    ResourceMapper.CreateMapping<MyResource>("things/{Id}");
    dynamic resource = new MyResource {Id = 1, Name = "Foo"}.AsResource();
    Assert.AreEqual("/things/1", resource._Href);
'''
