### Mendix Config Bootstarpper Buildpack

This project offers a way to expose configuring from Spring Cloud Config Server to your Mendix app as a config file (myconfig.yaml). Normally this requires a client library to be used in code such as those available for .NET via SteelToe or Java via Spring. This project allows config server to be used with Mendix app. Note that since configuration will be written to file in build stage, if values in config server changes you need to restart the app.

If config server binding is detected, it will replace any matching token in myconfig.yaml with config server value. Tokens are specified in format of `#{config:path}`

## How to Use

- Create Config server instance and bind it your your app

- Apply as supply buildpack in your manifest:

  ```yaml
  ---
  applications:
  - name: myapp
    buildpacks: 
    - https://github.com/kolluri-rk/mendix-config-bootstarpper-buildpack/releases/download/1.0/Pivotal.Mendix.Config.Bootstrapper.Buildpack-linux-x64-0.1.0.zip
    - nodejs_buildpack
    env:
      SPRING__APPLICATION__NAME: myapp
      SPRING__CLOUD__CONFIG__ENV: profile-name
      SPRING__CLOUD__CONFIG__LABEL: label-name
  ```

(Note if targeting windows, make sure to substitute above buildpack URL with appropriate windows based release. Get latest compiled buildpack binary URL on GitHub releases page)

Edit the env vars above:

* `SPRING__APPLICATION__NAME` - name of the app in config server
* `SPRING__CLOUD__CONFIG__ENV` - (optional) profile name
* `SPRING__CLOUD__CONFIG__LABEL` - (optional) label name

