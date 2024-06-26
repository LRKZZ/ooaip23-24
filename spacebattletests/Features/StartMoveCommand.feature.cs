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
    public partial class StartMoveCommandFeature : object, Xunit.IClassFixture<StartMoveCommandFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = ((string[])(null));
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "StartMoveCommand.feature"
#line hidden
        
        public StartMoveCommandFeature(StartMoveCommandFeature.FixtureData fixtureData, spacebattletests_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru"), "Features", "StartMoveCommand", "  Как система\r\n  Я хочу начать команду движения\r\n  Чтобы правильно инициировать п" +
                    "роцесс движения.", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        
        [Xunit.SkippableFactAttribute(DisplayName="Успешное выполнение команды начала движения")]
        [Xunit.TraitAttribute("FeatureTitle", "StartMoveCommand")]
        [Xunit.TraitAttribute("Description", "Успешное выполнение команды начала движения")]
        public virtual void УспешноеВыполнениеКомандыНачалаДвижения()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Успешное выполнение команды начала движения", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 8
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
#line 9
    testRunner.Given("я выполняю команду начала движения для успешного выполнения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line hidden
#line 10
    testRunner.When("команда успешно выполняется", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line hidden
#line 11
    testRunner.Then("она добавляется в очередь", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line hidden
#line 12
    testRunner.And("устанавливаются необходимые свойства", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "И ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Невозможно установить свойства")]
        [Xunit.TraitAttribute("FeatureTitle", "StartMoveCommand")]
        [Xunit.TraitAttribute("Description", "Невозможно установить свойства")]
        public virtual void НевозможноУстановитьСвойства()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Невозможно установить свойства", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 14
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
#line 15
    testRunner.Given("я выполняю команду начала движения для успешного выполнения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line hidden
#line 16
    testRunner.When("не могу установить свойства", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line hidden
#line 17
    testRunner.Then("возникает исключение", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Невозможно добавить команду в очередь")]
        [Xunit.TraitAttribute("FeatureTitle", "StartMoveCommand")]
        [Xunit.TraitAttribute("Description", "Невозможно добавить команду в очередь")]
        public virtual void НевозможноДобавитьКомандуВОчередь()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Невозможно добавить команду в очередь", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 19
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
#line 20
    testRunner.Given("я выполняю команду начала движения для успешного выполнения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line hidden
#line 21
    testRunner.When("не могу добавить команду в очередь", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line hidden
#line 22
    testRunner.Then("очередь остаётся пустой", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Невозможно прочитать свойства заказа")]
        [Xunit.TraitAttribute("FeatureTitle", "StartMoveCommand")]
        [Xunit.TraitAttribute("Description", "Невозможно прочитать свойства заказа")]
        public virtual void НевозможноПрочитатьСвойстваЗаказа()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Невозможно прочитать свойства заказа", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 24
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
#line 25
    testRunner.Given("я выполняю команду начала движения для успешного выполнения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line hidden
#line 26
    testRunner.When("не могу прочитать свойства заказа", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line hidden
#line 27
    testRunner.Then("возникает исключение", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Невозможно прочитать заказ")]
        [Xunit.TraitAttribute("FeatureTitle", "StartMoveCommand")]
        [Xunit.TraitAttribute("Description", "Невозможно прочитать заказ")]
        public virtual void НевозможноПрочитатьЗаказ()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Невозможно прочитать заказ", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 29
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
#line 30
    testRunner.Given("я выполняю команду начала движения для успешного выполнения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Допустим ");
#line hidden
#line 31
    testRunner.When("не могу прочитать заказ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line hidden
#line 32
    testRunner.Then("возникает исключение", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Внедрение команды в заменяемую команду в StartMoveCommand")]
        [Xunit.TraitAttribute("FeatureTitle", "StartMoveCommand")]
        [Xunit.TraitAttribute("Description", "Внедрение команды в заменяемую команду в StartMoveCommand")]
        public virtual void ВнедрениеКомандыВЗаменяемуюКомандуВStartMoveCommand()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Внедрение команды в заменяемую команду в StartMoveCommand", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 34
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
#line 35
    testRunner.Given("заменяемая команда и внедряемая команда созданы для начала движения", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Дано ");
#line hidden
#line 36
    testRunner.When("внедряемая команда внедряется в заменяемую команду StartMoveCommand", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Когда ");
#line hidden
#line 37
    testRunner.Then("заменяемая команда не выполняется в StartMoveCommand", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "То ");
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
                StartMoveCommandFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                StartMoveCommandFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
