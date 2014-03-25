using System.Collections.Generic;

namespace MiniTrello.Domain.Entities
{
    public class Organization : IEntity
    {
        private readonly IList<Board> _boards = new List<Board>();
        public virtual long Id { get; set; }
        public virtual bool IsArchived { get; set; }

        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual IEnumerable<Board> Boards { get { return _boards; } }

        public virtual void AddBoard(Board board)
        {
            if (!_boards.Contains(board))
            {
                _boards.Add(board);
            }
        }

        //aqui
        public virtual Account Administrador { get; set; }
        private readonly IList<Account> _members = new List<Account>();
        public virtual string Title { get; set; }
        public virtual string ShortName { get; set; }
        public virtual string Website { get; set; }
        public virtual bool PrivateOrPublic { get; set; }

        public virtual IEnumerable<Account> Members
        {
            get { return _members; }
        }

        public virtual void AddMember(Account member)
        {
            if (!_members.Contains(member))
            {
                _members.Add(member);
            }
        }
    }
}