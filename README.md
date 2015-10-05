# Mongo data center aware ObjectId

This is just an idea on MongoDB ObjectId

If you want to split data on multiple shards using only the _id field

```js
sh.enableSharding("demo");
sh.shardCollection("demo.foo", { _id : 1 } );
sh.addTagRange("demo.foo", { _id: MinKey }, { _id: ObjectId("000064000000000000000000") }, "DC1");
sh.addTagRange("demo.foo", { _id: ObjectId("000064000000000000000000") }, { _id: MaxKey }, "DC2");
```

When the app starts you have to set the right value for the machine identifier to target data to desired data-center using only the _id field
```csharp
DataCenterAwareIdGenerator.DataCenter = 200;
```
