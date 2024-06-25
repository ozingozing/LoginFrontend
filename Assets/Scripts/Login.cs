using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
	[SerializeField] private string authenticationEndpoint = "http://127.0.0.1:13756/account";

	[SerializeField] private TextMeshProUGUI alertText;
	[SerializeField] private Button loginButton;
    [SerializeField] private TMP_InputField usernameInputField;
	[SerializeField] private TMP_InputField passwordInputField;

	public void OnLoginClick()
	{
		alertText.text = "Singin in...";
		loginButton.interactable = false;
		StartCoroutine(TryLogin());
	}

	private IEnumerator TryLogin()
	{

		string username = usernameInputField.text;
		string password = passwordInputField.text;

		UnityWebRequest request = UnityWebRequest.Get($"{authenticationEndpoint}?rUsername={username}&rPassword={password}");
		var handler = request.SendWebRequest();

		float startTime = 0.0f;
		while (!handler.isDone) 
		{
			startTime += Time.deltaTime;

			if(startTime > 10.0f)
			{
				break;
			}

			yield return null; 
		}

		if(request.result == UnityWebRequest.Result.Success) 
		{
			if(request.downloadHandler.text != "Invalid credentials") //login success?
			{
				alertText.text = "Welcome!!";
				loginButton.interactable = false;
			}
			else
			{
				alertText.text = "Invalid credentials";
				loginButton.interactable = true;
			}
		}
		else
		{
			alertText.text = "Error connecting to the server...";
			loginButton.interactable = true;
		}

		yield return null;
	}
}
