using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Vaquinha.Tests.Common.Fixtures;
using Xunit;

namespace Vaquinha.AutomatedUITests
{
	public class DoacaoTests : IDisposable, IClassFixture<DoacaoFixture>, 
                                               IClassFixture<EnderecoFixture>, 
                                               IClassFixture<CartaoCreditoFixture>
	{
		private DriverFactory _driverFactory = new DriverFactory();
		private IWebDriver _driver;

		private readonly DoacaoFixture _doacaoFixture;
		private readonly EnderecoFixture _enderecoFixture;
		private readonly CartaoCreditoFixture _cartaoCreditoFixture;

		public DoacaoTests(DoacaoFixture doacaoFixture, EnderecoFixture enderecoFixture, CartaoCreditoFixture cartaoCreditoFixture)
        {
            _doacaoFixture = doacaoFixture;
            _enderecoFixture = enderecoFixture;
            _cartaoCreditoFixture = cartaoCreditoFixture;
        }
		public void Dispose()
		{
			_driverFactory.Close();
		}

		[Fact]
		public void DoacaoUI_AcessoTelaHome()
		{
			// Arrange
			_driverFactory.NavigateToUrl("https://vaquinha.azurewebsites.net/");
			_driver = _driverFactory.GetWebDriver();

			// Act
			IWebElement webElement = null;
			webElement = _driver.FindElement(By.ClassName("vaquinha-logo"));

			// Assert
			webElement.Displayed.Should().BeTrue(because:"logo exibido");
		}

		[Fact]
		public void DoacaoUI_CriacaoDoacao()
		{
			//Arrange
			var doacao = _doacaoFixture.DoacaoValida();
            doacao.AdicionarEnderecoCobranca(_enderecoFixture.EnderecoValido());
            doacao.AdicionarFormaPagamento(_cartaoCreditoFixture.CartaoCreditoValido());
			_driverFactory.NavigateToUrl("https://vaquinha.azurewebsites.net/");
			_driver = _driverFactory.GetWebDriver();

			//Act
			IWebElement webElement = null;
			webElement = _driver.FindElement(By.ClassName("btn-yellow"));
			webElement.Click();

			IWebElement nome = _driver.FindElement(By.Id("DadosPessoais_Nome"));
			nome.SendKeys(doacao.DadosPessoais.Nome);
			
			IWebElement email = _driver.FindElement(By.Id("DadosPessoais_Email"));
			email.SendKeys(doacao.DadosPessoais.Email);
			
			IWebElement mensagem = _driver.FindElement(By.Id("DadosPessoais_MensagemApoio"));
			mensagem.SendKeys(doacao.DadosPessoais.MensagemApoio);
			
			IWebElement textoEndereco = _driver.FindElement(By.Id("EnderecoCobranca_TextoEndereco"));
			textoEndereco.SendKeys(doacao.EnderecoCobranca.TextoEndereco);
			
			IWebElement cidade = _driver.FindElement(By.Id("EnderecoCobranca_Cidade"));
			cidade.SendKeys(doacao.EnderecoCobranca.Cidade);
			
			IWebElement cep = _driver.FindElement(By.Id("cep"));
			cep.SendKeys(doacao.EnderecoCobranca.CEP);
			
			IWebElement numero = _driver.FindElement(By.Id("EnderecoCobranca_Numero"));
			numero.SendKeys(doacao.EnderecoCobranca.Numero);
			
			IWebElement estado = _driver.FindElement(By.Id("estado"));
			estado.SendKeys(doacao.EnderecoCobranca.Estado);
			
			IWebElement complemento = _driver.FindElement(By.Id("EnderecoCobranca_Complemento"));
			complemento.SendKeys(doacao.EnderecoCobranca.Complemento);
			
			IWebElement telefone = _driver.FindElement(By.Id("telefone"));
			telefone.SendKeys(doacao.EnderecoCobranca.Telefone);

			webElement = _driver.FindElement(By.ClassName("btn-yellow"));
			webElement.Click();
			
			//Assert
			_driver.Url.Should().Contain("/Doacoes/Create");
		}
	}
}