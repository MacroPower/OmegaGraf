# Contributing

## Introduction

Thank you for considering contributing to OmegaGraf.

No contribution is too small. Please help with:

* Reporting bugs
* Suggesting features
* Writing or improving documentation
* Fixing typos
* Cleaning whitespace
* Refactoring code
* Adding tests
* Improvements to the [website](https://github.com/OmegaGraf/omegagraf.github.io)
* Improvements to our [sim](https://github.com/OmegaGraf/docker-vcsim)
* Adding Dashboards
* Closing issues

## Ground Rules

* Ensure cross-platform compatibility for every change that's accepted. Windows, Debian & Ubuntu Linux.
* Create issues for any major changes and enhancements that you wish to make. Discuss things transparently and get community feedback.
* Keep feature versions as small as possible, preferably one new feature per version.
* Be welcoming to newcomers and encourage diverse new contributors from all backgrounds.

## Your First Contribution

Unsure where to begin contributing? You can start by looking through these beginner and help-wanted issues:

* Beginner issues - issues which should only require a few lines of code, and a test or two.
* Help wanted issues - issues which should be a bit more involved than beginner issues.
* Both issue lists are sorted by total number of comments. While not perfect, number of comments is a reasonable proxy for impact a given change will have.

## Getting started

For something that is bigger than a one or two line fix:

1. Create your own fork of the code.
2. Do the changes in your fork.
3. If you like the change and think the project could use it:
    * Be sure you have followed the code style for the project.
    * Send a pull request.

---

## Github Style Guidelines

Recommendations for commit and pull request subjects.

### General

1. Avoid using punctuation.
2. Begin with a capital letter.
3. Follow our templates when applicable.

### Prefix

Please consider using one of the following prefixes:

1. Compose - Edits to the backend.
2. UI - Edits to the UI project.
3. /Tests - Edits to the tests.
4. Docs - Edits to any docs (e.g. readme).
5. Branding - Edits to any branding (e.g. logo).
6. Git - Edits to git config (e.g. gitignore).
7. Travis/Azure/Sonar - Edits to any build pipelines.
8. VS - VSCode config edits.
9. Meta - General edits (e.g. moving files, edits to tooling).

### Examples

* `Compose: Fix spelling on 'path' parameter help`
* `Compose/Tests: Add a test to ensure login fails for incorrect credentials`
* `Meta: Add cleanup parameter to build script`

---

## Pull Request Etiquette

Adapted from [pr_etiquette.md](https://gist.githubusercontent.com/mikepea/863f63d6e37281e329f8/raw/f127b247383132259225316d6fb556f92b4fef50/pr_etiquette.md)

## Why do we use a Pull Request workflow

PRs are a great way of sharing information, and can help us be aware of the changes that are occuring in our codebase. They are also an excellent way of getting peer review on the work that we do, without the cost of working in direct pairs.

**Ultimately though, the primary reason we use PRs is to encourage quality in the commits that are made to our code repositories.**

Done well, the commits (and their attached messages) contained within tell a story to people examining the code at a later date. If we are not careful to ensure the quality of these commits, we silently lose this ability.

**Poor quality code can be refactored. A terrible commit lasts forever.**

## What constitutes a good PR

A good quality PR will have the following characteristics:

* It will be a complete piece of work that adds value in some way.
* It will have a title that reflects the work within, and a summary that helps to understand the context of the change.
* There will be well written commit messages, with well crafted commits that tell the story of the development of this work.
* Ideally it will be small and easy to understand. Single commit PRs are usually easy to submit, review, and merge.
* The code contained within will meet the best practises set by the team wherever possible.

A PR does not end at submission though. A code change is not made until it is merged and used in production.

A good PR should be able to flow through a peer review system easily and quickly.

## Submitting Pull Requests

### Ensure there is a solid title and summary

PRs are a Github workflow tool, so it's important to understand that the PR title, summary and eventual discussion are not as trackable as the the commit history. If we ever move away from Github, we'll likely lose this infomation.

That said however, they are a very useful aid in ensuring that PRs are handled quickly and effectively.

Ensure that your PR title is scannable. People will read through the list of PRs attached to a repo, and must be able to distinguish between them based on title. Include a story/issue reference if possible, so the reviewer can get any extra context. Include a reference to the subsystem affected, if this is a large codebase.

Use keywords in the title to help people understand your intention with the PR, eg [WIP] to indicate that it's still in progress, so should not be merged.

### Rebase before you make the PR, if needed

Unless there is a good reason not to rebase - typically because more than one person has been working on the branch - it is often a good idea to rebase your branch to tidy up before submitting the PR.

Use `git rebase -i master # or other reference, eg HEAD~5`

For example:

* Merge 'oops, fix typo/bug' into their parent commit. There is no reason to create and solve bugs within a PR, **unless there is educational value in highlighting them**.
* Reword your commit messages for clarity. Once a PR is submitted, any rewording of commits will involve a rebase, which can then mess up the conversation in the PR.

### Aim for one succinct commit

In an ideal world, your PR will be one small(ish) commit, doing one thing - in which case your life will be made easier, since the commit message and PR title/summary are equivalent.

If your change contains more work than can be sensibly covered in a single commit though, **do not** try to squash it down into one. Commit history should tell a story, and if that story is long then it may require multiple commits to walk the reviewer through it.

### Describe your changes well in each commit

Commit messages are invaluable to someone reading the history of the code base, and are critical for understanding why a change was made.

Try to ensure that there is enough information in there for a person with no context or understanding of the code to make sense of the change.

Where external information references are available - such as Issue/Story IDs, PR numbers - ensure that they are included in the commit message.

Remember that your commit message must survive the ravages of time. Try to link to something that will be preserved equally well -- another commit for example, rather than linking to master.

**Each commit message should include the reason why this commit was made. Usually by adding a sentence completing the form 'So that we...' will give an amazing amount of context to the history that the code change itself cannot.**

### Keep it small

Try to only fix one issue or add one feature within the pull request. The larger it is, the more complex it is to review and the more likely it will be delayed. Remember that reviewing PRs is taking time from someone else's day.

If you must submit a large PR, try to at least make someone else aware of this fact, and arrange for their time to review and get the PR merged. It's not fair to the team to dump large pieces of work on their laps without warning.

If you can rebase up a large PR into multiple smaller PRs, then do so.

## Reviewing Pull Requests

It's a reviewers responsibility to ensure:

* Commit history is excellent
* Good changes are propagated quickly
* Code review is performed
* They understand what is being changed, from the perspective of someone examining the code in the future.

### Reviewers are the guardians of the commit history

The importance of ensuring a quality commit history cannot be stressed enough. It is the historical context of all of the work that we do, and is vital for understanding the reasons why changes were made in the past. What is obvious now, will not be obvious 2-3 years in the future.

Without a decent commit history, we may as well be storing all our code in files ending yyyy-mm-dd. The commit history of a code base is what allows people to understand **why** a change was made - the when, what, and where are automatically evident.

When looking at a commit message, ask yourself the question - from the perspective of someone looking at this change without any knowledge of the codebase - 'do I understand *why* this change was made?'

**If any commit within the PR does not meet this standard, the PR should be rebased until it does. We cannot fix a commit history once it is in place, unlike our ability to refactor crappy code or fix bugs.**

A useful tip is simply asking the submitter to add a sentence to the commit message completing the sentence 'So that we...'.

### Keep the flow going

Pull Requests are the fundamental unit of how we progress change. If PRs are getting clogged up in the system, either unreviewed or unmanaged, they are preventing a piece of work from being completed.

As PRs clog up in the system, merges become more difficult, as other features and fixes are applied to the same codebase. This in turn slows them down further, and often completely blocks progress on a given codebase.

There is a balance between flow and ensuring the quality of our PRs. As a reviewer you should make a call as to whether a code quality issue is sufficient enough to block the PR whilst the code is improved. Possibly it is more prudent to simply flag that the code needs rework, and raise an issue.

Any quality issue that will obviously result in a bug should be fixed.

### We are all reviewers

If you are the first to comment on a PR, you are that PRs reviewer. If you feel that you can no longer be responsible for the subsequent merge or closure of the PR, then flag this up in the PR conversation, so someone else can take up the role.

There's no reason why multiple people cannot comment on a PR and review it, and this is to be encouraged.

### Don't add to the PR yourself

It's sometimes tempting to fix a bug in a PR yourself, or to rework a section to meet coding standards, or just to make a feature better fit your needs.

If you do this, you are no longer the reviewer of the PR. You are a collaborator, and so should not merge the PR.

It is of course possible to find a new reviewer, but generally change will be speedier if you require the original submitter to fix the code themselves. Alternatively, if the original PR is 'good enough', raise the changes you'd like to see as separate stories/issues, and rework in your own PR.

### It is not the reviewers responsibility to test the code

We are all busy people, and in the case of many PRs against our codebase we are not able or time-permitted to test the new code.

We need to assume that the submitter has tested their code to the point of being happy with the work to be merged to master and subsequently released.

If you, as a reviewer, are suspicious that the work in the PR has not been tested, raise this with the submitter. Find out how they have tested it, and refuse the work if they have not. They may not have a mechanism to test it, in which case you may need to help.

If, as a submitter, you know that this change is not fully tested, highlight this in the PR text, and talk to the reviewer.
