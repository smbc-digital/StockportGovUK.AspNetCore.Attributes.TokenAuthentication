<h1 align="center">StockportGovUK.AspNetCore.Attributes.TokenAuthentication</h1>

<div align="center">
  :computer::door::rainbow:
</div>
<div align="center">
  <strong>Wonders lie ahead</strong>
</div>
<div align="center">
  The token authentication attribute enables simple basic token authentication, this will prohibit access to endpoints based on a client providing an API Key. 
</div>

<br />


<div align="center">
  <h3>
    External Links
    <a href="https://github.com/smbc-digital/StockportGovUK.AspNetCore.Attributes.TokenAuthentication">
      GitHub
    </a>
    <span> | </span>
    <a href="https://www.nuget.org/packages/StockportGovUK.AspNetCore.Attributes.TokenAuthentication/">
      NuGet
    </a>
  </h3>
</div>

<div align="center">
  <sub>Built with ❤︎ by
  <a href="https://www.stockport.gov.uk">Stockport Council</a> and
  <a href="">
    contributors
  </a>
</div>

## Defaults for clients ##


By default the key can either be as the query string "api_key" or in the "Authorization" header with the format:

```
Authorization: BEARER YourSecretTokenHere
```

## Usage ##

To secure an API end point or an entire controller using the TokenAuthentication attribute.

```C#
[TokenAuthentication]
public ActionResult YourActionName()
```

## Configuration ##

The required API key is stored in a preferrably secret configuration file for the service in the format below:

```json
"TokenAuthentication": {
    "Key": "Your secret token here",
}
```

You can also specify an alternative querystring parameter name.

```json
"TokenAuthentication": {
    "Key": "Your secret token here",
    "QueryString": "MyCustomQueryString"
}
```

Or an alternative custom header.

```json
"TokenAuthentication": {
    "Key": "Your secret token here",
    "Header": "MyCustomHeader"
}
```

### Ignored Routes ###
You can specify routes to be ignored when you specify TokenAuthentication and the controller level as below.
```c#
[TokenAuthentication(IgnoredRoutes = new []{"/api/my/endpoint/action"})]
```

### Expected Results ###
Successful requests will result in processes continuing to execute.

Incorrect or non-existant API keys for authenticated end points with return an UnauthorizedObjectResult (401)

Any issues encountered during the processing of the request will result in a BadRequestObjectResult (500)

### Custom Redirects ###
Rather than return a 401 code you can specify a custom redirect, unauthorized requests will be redirected to the specified url.

```json
    "TokenAuthentication": {
        "Key": "abc12345",
        "CustomRedirect": "https://www.stockport.gov.uk"
    }
```