using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace GitSongs
{
    class Program
    {
        static void Main(string[] args)
        {
            var workingDir = @"F:\Projects\Utility.Click\";

            using (var repo = new Repository(workingDir))
            {
                var length = (repo.Commits.FirstOrDefault().Committer.When.DateTime - repo.Commits.LastOrDefault().Committer.When.DateTime).Ticks / 100000;

                var tempo = (int)(length / repo.Commits.Count());
                var actions = new List<Action>();
                var count = 1;
                foreach (var branch in repo.Branches)
                {
                    actions.Add(new Action(() => PlayBranch(tempo, branch, $"{repo.Branches.ToList().IndexOf(branch) + 1}.wav")));
                    count++;
                }

                Parallel.Invoke(actions.ToArray());
            }
        }

        private static void PlayBranch(int tempo, Branch branch, string sound)
        {
            var commits = branch.Commits.ToList();
            foreach (var commit in commits)
            {
                var prevCommit = commits[Math.Max(0, commits.IndexOf(commit) - 1)];
                var length = (prevCommit.Committer.When.DateTime - commit.Committer.When.DateTime).Ticks / 100000;
                var delay = (int)(length / commits.Count());
                var player = new System.Media.SoundPlayer(sound);
                player.Play();
                System.Threading.Thread.Sleep((tempo + delay) / 100);



            }
        }
    }
}
