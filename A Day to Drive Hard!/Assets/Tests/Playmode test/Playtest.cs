﻿using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class Playtest {

    [Test]
    public void PlaytestSimplePasses() {
        
    }

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator PlaytestWithEnumeratorPasses() {
        
        yield return null;
    }
}