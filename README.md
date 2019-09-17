The token authentication attribute enables simple basic token authentication which will prohibit access to endpoints based on a client providing a key either as "api_key" in the request query string or in the "Authorization" header in the format;

```
Authorization: BEARER YourSecretTokenHere
```

The required API key is stored in a preferrably secret configuration file for the service in the format below:

```json
"TokenAuthentication": {
    "Key": "Your secret token here",
}
```

You can also specify an alternative value to look for in the querystring.

```json
"TokenAuthentication": {
    "Key": "Your secret token here",
    "QueryString": "MyCustomQueryString"
}
```

Or an alternative header.

```json
"TokenAuthentication": {
    "Key": "Your secret token here",
    "Header": "MyCustomHeader"
}
```


To secure an API end point or an entire controller using the TokenAuthentication attribute.

```C#
[TokenAuthentication]
public ActionResult YourActionName()
```

### Ignored Routes ###
You can specify routes to be ignored when you specify TokenAuthentication and the controller level as below.
```c#
IgnoredRoutes = new []{"/api/my/endpoint/action"})
```
