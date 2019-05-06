# Sentiance SDK Starter application for Xamarin
A simple single-view application that uses the Sentiance SDK.

## To run this project:
1. Request a developer account by [contacting Sentiance](mailto:support@sentiance.com).
2. Grab your test app credentials from [here](https://insights.sentiance.com/#/apps).
4. Install dependencies with `nuget install packages.config -o packages`.
5. [Download the latest Sentiance Android Xamarin SDK](https://docs.sentiance.com/sdk/appendix/xamarin) and place it in the `Libs` folder.
6. Open the `.sln` file in Xamarin Studio.
7. Make sure the DLL file is correctly added to the References.
8. In `MyApplication.cs`: fill in the `APP_ID` and `APP_SECRET` variables with the credentials from the application you added in step 2.
9. Using Xamarin Studio, you can now build and run the application on your device.


## More info
- [Full documentation on the Sentiance SDK](https://docs.sentiance.com/)
