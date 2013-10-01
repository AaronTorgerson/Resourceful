Resourceful
===========

A poorly-named library to help you serialize your models with hypermedia URIs.


    public class MyResource
    {
    	public int Id { get; set; }
    	public string Name { get; set; }
    }
  
    ResourceMapper.CreateMapping<MyResource>("things/{Id}");
    dynamic resource = new MyResource {Id = 1, Name = "Foo"}.AsResource();
    Assert.AreEqual("/things/1", resource._Href);
		
