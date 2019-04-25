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

To secure an API end point use the TokenAuthentication attribute.

```C#
[TokenAuthentication]
public ActionResult YourActionName()
```