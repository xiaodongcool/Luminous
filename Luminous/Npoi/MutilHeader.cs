using System.Collections.Generic;
using System.Linq;

namespace Luminous
{
    /// <summary>
    ///     构建多级表头
    /// </summary>
    public class MutilHeader
    {
        public MutilHeader(string name)
        {
            Title = name;
        }

        public MutilHeader(string name, params string[] children)
        {
            Title = name;

            if (children != null)
            {
                Children = children.Select(x => new MutilHeader(x)).ToList();
            }
        }

        public MutilHeader(string name, params MutilHeader[] children)
        {
            Title = name;

            if (children != null)
            {
                Children = children.ToList();
            }
        }

        /// <summary>
        ///     标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     列宽倍数
        /// </summary>
        public double WidthMultiples { get; set; }

        /// <summary>
        ///     占用几行
        /// </summary>
        public int CrossRowCount { get; set; }

        /// <summary>
        ///     下级表头
        /// </summary>
        public List<MutilHeader> Children { get; set; } = new List<MutilHeader>();

        public MutilHeader AddChild(string name)
        {
            var child = new MutilHeader(name);

            Children.Add(child);

            return this;
        }

        public MutilHeader AddChild(MutilHeader child)
        {
            Children.Add(child);

            return this;
        }

        public MutilHeader AddChildReturnChild(string name)
        {
            var child = new MutilHeader(name);

            Children.Add(child);

            return child;
        }

        public MutilHeader AddChildReturnChild(MutilHeader child)
        {
            Children.Add(child);

            return child;
        }

        public MutilHeader AddChildren(params string[] children)
        {
            foreach (var child in children)
            {
                AddChild(child);
            }

            return this;
        }

        public MutilHeader AddChildren(params MutilHeader[] children)
        {
            foreach (var child in children)
            {
                AddChild(child);
            }

            return this;
        }

        public MutilHeader AddChildrenRecursively(params string[] children)
        {
            if (children.Length == 1)
            {
                return AddChildReturnChild(children[0]);
            }
            else
            {
                var header = AddChildReturnChild(children[0]);

                foreach (var child in children.Skip(1))
                {
                    header = header.AddChildReturnChild(child);
                }

                return header;
            }
        }

        public MutilHeader AddChildrenRecursively(params MutilHeader[] children)
        {
            if (children.Length == 1)
            {
                return AddChildReturnChild(children[0]);
            }
            else
            {
                var header = AddChildReturnChild(children[0]);

                foreach (var child in children.Skip(1))
                {
                    header = header.AddChildReturnChild(child);
                }

                return header;
            }
        }
    }
}
