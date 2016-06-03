# Sentiance SDK Starter application for Xamarin
A simple single-view application that uses the Sentiance SDK and allows the user to manually control when detections occur.

## To run this project:
1. Clone this repository and `cd` into it.
2. [Create a developer account here](https://audience.sentiance.com/developers).
3. [Register a Sentiance application here](https://audience.sentiance.com/apps) to obtain an application ID and secret.
4. Install dependencies with `nuget install packages.config -o packages`.
5. [Download the Sentiance Android Xamarin SDK](https://s3-eu-west-1.amazonaws.com/sentiance-sdk/android/xamarin/SENTTransportDetection.Droid.dll) and place it in the `Libs` folder.
6. Open the `.sln` file in Xamarin Studio.
5. In `MyApplication.cs`: fill in the `APP_ID` and `APP_SECRET` variables with the credentials from the application you added in step 3.
6. Using Xamarin Studio, you can now build and run the application on your device.


## More info
- [Full documentation on the Sentiance SDK](https://audience.sentiance.com/docs)
- [Developer signup](https://audience.sentiance.com/developers)
