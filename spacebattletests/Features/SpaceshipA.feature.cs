﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.9.0.0
//      SpecFlow Generator Version:3.9.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace spacebattletests.Features
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class RotatingActionFeature : object, Xunit.IClassFixture<RotatingActionFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = ((string[])(null));
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "SpaceshipA.feature"
#line hidden
        
        public RotatingActionFeature(RotatingActionFeature.FixtureData fixtureData, spacebattletests_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru"), "Features", "RotatingAction", null, ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Игровой объект может вращаться вокруг собственной оси")]
        [Xunit.TraitAttribute("FeatureTitle", "RotatingAction")]
        [Xunit.TraitAttribute("Description", "Игровой объект может вращаться вокруг собственной оси")]
        public virtual void ИгровойОбъектМожетВращатьсяВокругСобственнойОси()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Игровой объект может вращаться вокруг собственной оси", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 5
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 6
 testRunner.Given("космический корабль имеет угол наклона 45 град к оси OX", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line hidden
#line 7
 testRunner.And("имеет мгновенную угловую скорость 45 град", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
#line 8
 testRunner.When("происходит вращение вокруг собственной оси", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line hidden
#line 9
 testRunner.Then("угол наклона космического корабля к оси OX составляет 90 град", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Если невозможно определить угол наклона к оси OX космического корабля, то вращени" +
            "е вокруг собственной оси  невозможно")]
        [Xunit.TraitAttribute("FeatureTitle", "RotatingAction")]
        [Xunit.TraitAttribute("Description", "Если невозможно определить угол наклона к оси OX космического корабля, то вращени" +
            "е вокруг собственной оси  невозможно")]
        public virtual void ЕслиНевозможноОпределитьУголНаклонаКОсиOXКосмическогоКорабляТоВращениеВокругСобственнойОсиНевозможно()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Если невозможно определить угол наклона к оси OX космического корабля, то вращени" +
                    "е вокруг собственной оси  невозможно", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 11
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 12
 testRunner.Given("космический корабль, угол наклона которого невозможно определить", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line hidden
#line 13
 testRunner.And("имеет мгновенную угловую скорость 45 град", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
#line 14
 testRunner.When("происходит вращение вокруг собственной оси", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line hidden
#line 15
 testRunner.Then("возникает ошибка вращения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Если невозможно определить мгновенную угловую скорость космического корабля, то в" +
            "ращение вокруг собственной оси  невозможно")]
        [Xunit.TraitAttribute("FeatureTitle", "RotatingAction")]
        [Xunit.TraitAttribute("Description", "Если невозможно определить мгновенную угловую скорость космического корабля, то в" +
            "ращение вокруг собственной оси  невозможно")]
        public virtual void ЕслиНевозможноОпределитьМгновеннуюУгловуюСкоростьКосмическогоКорабляТоВращениеВокругСобственнойОсиНевозможно()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Если невозможно определить мгновенную угловую скорость космического корабля, то в" +
                    "ращение вокруг собственной оси  невозможно", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 17
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 18
 testRunner.Given("космический корабль имеет угол наклона 45 град к оси OX", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line hidden
#line 19
 testRunner.And("мгновенную угловую скорость невозможно определить", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
#line 20
 testRunner.When("происходит вращение вокруг собственной оси", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line hidden
#line 21
 testRunner.Then("возникает ошибка вращения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Если невозможно установить новый угол наклона космического корабля космического к" +
            "орабля, то вращение вокруг собственной оси  невозможно")]
        [Xunit.TraitAttribute("FeatureTitle", "RotatingAction")]
        [Xunit.TraitAttribute("Description", "Если невозможно установить новый угол наклона космического корабля космического к" +
            "орабля, то вращение вокруг собственной оси  невозможно")]
        public virtual void ЕслиНевозможноУстановитьНовыйУголНаклонаКосмическогоКорабляКосмическогоКорабляТоВращениеВокругСобственнойОсиНевозможно()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Если невозможно установить новый угол наклона космического корабля космического к" +
                    "орабля, то вращение вокруг собственной оси  невозможно", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 23
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 24
 testRunner.Given("космический корабль имеет угол наклона 45 град к оси OX", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line hidden
#line 25
 testRunner.And("имеет мгновенную угловую скорость 45 град", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
#line 26
 testRunner.And("невозможно изменить уголд наклона к оси OX космического корабля", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
#line 27
 testRunner.When("происходит вращение вокруг собственной оси", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line hidden
#line 28
 testRunner.Then("возникает ошибка вращения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Тогда ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                RotatingActionFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                RotatingActionFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
