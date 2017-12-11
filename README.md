# Sentiance SDK Starter application for Xamarin
A simple single-view application that uses the Sentiance SDK.

## To run this project:
1. Clone this repository and `cd` into it.
2. [Create a developer account here](https://audience.sentiance.com/developers).
3. [Register a Sentiance application here](https://audience.sentiance.com/apps) to obtain an application ID and secret.
4. Install dependencies with `nuget install packages.config -o packages`.
5. [Download the Sentiance Android Xamarin SDK](https://sentiance-sdk.s3.amazonaws.com/android/xamarin/sentiance-android-sdk-3.11.3.dll) and place it in the `Libs` folder.
6. Open the `.sln` file in Xamarin Studio.
7. Make sure the DLL file is correctly added to the References.
8. In `MyApplication.cs`: fill in the `APP_ID` and `APP_SECRET` variables with the credentials from the application you added in step 3.
9. Using Xamarin Studio, you can now build and run the application on your device.


## More info
- [Full documentation on the Sentiance SDK](https://audience.sentiance.com/docs)
- [Developer signup](https://audience.sentiance.com/developers)
