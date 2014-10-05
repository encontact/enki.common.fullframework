using Castle.ActiveRecord;
namespace Enki.Common {
    /// <summary>
    /// Estrutura base para objetos de modelo do sistema.
    /// </summary>
	public abstract class BaseModel<T, D> where D : ActiveRecordBase<D> {
		public int Id { get; set; }
        /// <summary>
        /// Definição para método de gravação do objeto.
        /// </summary>
		public abstract void Save();
        /// <summary>
        /// Definição para método de gravação do objeto com os objetos de dependência.
        /// </summary>
		public abstract void SaveWithDependencies();
	}
}
