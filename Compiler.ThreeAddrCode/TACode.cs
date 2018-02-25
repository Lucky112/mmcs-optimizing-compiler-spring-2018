using System;
using System.Collections.Generic;
using System.Linq;
using Compiler.ThreeAddrCode.Nodes;

namespace Compiler.ThreeAddrCode
{
    /// <summary>
    ///     Класс для хранения программы в формате трехадресного кода
    ///     Трехадресный код имеет следующий вид:
    ///     <para>Result := Left Operation Right; Result, Left, Right - операнды, Operation - бинарная операция</para>
    ///     <para>Код программы хранится в виде команд в формате трехадресного кода в списке</para>
    /// </summary>
    public class TACode
    {
        /// <summary>
        /// </summary>
        private readonly List<Node> m_code;

        private readonly Dictionary<Guid, Node> m_labeledCode;

        public TACode()
        {
            m_code = new List<Node>();
            m_labeledCode = new Dictionary<Guid, Node>();
        }

        public IEnumerable<Node> CodeList
        {
            get { return m_code; }
        }

        /// <summary>
        ///     Добавить оператор
        /// </summary>
        public void AddNode(Node node)
        {
            m_code.Add(node);
            m_labeledCode.Add(node.Label, node);
        }


        /// <summary>
        ///     Удалить оператор
        /// </summary>
        /// <param name="node">Оператор</param>
        public bool RemoveNode(Node node)
        {
            return m_code.Remove(node);
        }

        /// <summary>
        ///     Удалить набор операторов
        /// </summary>
        /// <param name="nodes">Список</param>
        public void RemoveRange(IEnumerable<Node> nodes)
        {
            foreach (var node in nodes)
                m_code.Remove(node);
        }

        public override string ToString()
        {
            return m_code.Aggregate("", (s, node) => s + node.ToString() + Environment.NewLine);
        }
    }
}