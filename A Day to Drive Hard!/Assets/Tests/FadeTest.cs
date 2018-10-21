using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NUnit.Framework;


public class FadeTest{

    [Test]
    public void FadeInTest()
    {
        GameObject camera = new GameObject();
        camera.AddComponent<Camera>();
        GameObject canvas = new GameObject();
        canvas.AddComponent<Canvas>();

        GameObject fadeInText = new GameObject();
        fadeInText.transform.parent = canvas.transform;
        fadeInText.AddComponent<Text>();
        fadeInText.GetComponent<Text>().text = "This text is fading";

        Color test = fadeInText.GetComponent<Text>().color;
        test.a = 0;
        fadeInText.GetComponent<Text>().color = test;

        CombinedMovementScript.FadeInOutText(fadeInText.GetComponent<Text>());

        Assert.GreaterOrEqual(fadeInText.GetComponent<Text>().color.a, 0.9f);
    }

    [Test]
    public void FadeInOutTest()
    {
        GameObject camera = new GameObject();
        camera.AddComponent<Camera>();
        GameObject canvas = new GameObject();
        canvas.AddComponent<Canvas>();

        GameObject fadeInText = new GameObject();
        fadeInText.transform.parent = canvas.transform;
        fadeInText.AddComponent<Text>();
        fadeInText.GetComponent<Text>().text = "This text is fading";

        Color test = fadeInText.GetComponent<Text>().color;
        test.a = 0;
        fadeInText.GetComponent<Text>().color = test;

        CombinedMovementScript.FadeInOutText(fadeInText.GetComponent<Text>());

        Assert.LessOrEqual(fadeInText.GetComponent<Text>().color.a, 0.1f);
    }

}
